public class MicroProcessorControl : IMicroProcessorControl
{
    ICameraService _cameraService;
    IDoorService _doorService;
    ILightService _lightService;
    IWindowService _windowService;
    HardwareControl(ICameraService cameraService, IDoorService doorService, ILightService lightService, IWindowService windowService)
    {
        _cameraService = cameraService;
        _doorService = doorService;
        _lightService = lightService;
        _windowService = windowService;
    }
    public async Task<BaseResponse> UpdateHardwareServices(UpdateHardwareDto hardwareDto)
    {
        if (hardwareDto != null)
        {
            foreach (var updateCamera in hardwareDto.updateCameraDtos) await _cameraService.UpdateCamera(updateCamera);
            foreach (var updateDoor in hardwareDto.updateDoorDtos) await _doorService.UpdateDoor(updateDoor);
            foreach (var updateLight in hardwareDto.updateLightDtos) await _lightService.UpdateLight(updateLight);
            foreach (var updateWindow in hardwareDto.updateWindowDtos) await _windowService.UpdateWindow(updateWindow);
            return new BaseResponse()
            {
                Message = "Hardware Updated!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Message = "Unable to update Hardware!",
            Status = false
        };
    }
    public async Task<HardwareResponse> GetHardwareServices(UpdateHardwareResponseIds updateHardwareResponseIds)
    {
        if (updateHardwareResponseIds != null)
        {
            var hardwareResponse = new HardwareResponse();
            foreach (var item in updateHardwareResponseIds.cameraIds) hardwareResponse.getCameraDtos.Add((await _cameraService.GetById(item)).Data);
            foreach (var item in updateHardwareResponseIds.doorIds) hardwareResponse.getDoorDtos.Add((await _doorService.GetById(item)).Data);
            foreach (var item in updateHardwareResponseIds.lightIds) hardwareResponse.getLightDtos.Add((await _lightService.GetById(item)).Data);
            foreach (var item in updateHardwareResponseIds.windowIds) hardwareResponse.getWindowDtos.Add((await _windowService.GetById(item)).Data);
            hardwareResponse.Message = "Data Retrieved Successfuly!";
            hardwareResponse.Status = true;
            return hardwareResponse;
        }
        return new HardwareResponse()
        {
            Message = "Unable to retreive hardware data!",
            Status = false
        };
    }