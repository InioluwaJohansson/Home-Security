using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class FingerPrintService : IFingerPrintService
{
    IFingerPrintRepo _fingerPrintRepo;
    public FingerPrintService(IFingerPrintRepo fingerPrintRepo)
    {
        _fingerPrintRepo = fingerPrintRepo;
    }
    public async Task<FingerPrintResponseModel> VerifyFingerPrint(byte [] data)
    {
        var fingerprint = await _fingerPrintRepo.Get(x => x.FingerPrintEncoding.Equals(data) && x.IsDeleted == false);
        if (data != null)
        {
            return new FingerPrintResponseModel
            {
                Data = new GetFingerPrintDto()
                {
                    Id = fingerprint.Id,
                    FingerPrintId = fingerprint.FingerPrintId,
                    PersonId = fingerprint.PersonId,
                },
                Status = true,
                Message = "FingerPrint Verified"
            };
        }
        return new FingerPrintResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Find FingerPrint!"
        };
    }
    public async Task<FingerPrintResponseModel> VerifyFingerPrint(VerifyFingerPrintDto dto)
    {
        var allFingerprints = await _fingerPrintRepo
            .GetAll(x => x.IsDeleted == false);

        if (allFingerprints == null || !allFingerprints.Any())
        {
            return new FingerPrintResponseModel
            {
                Status = false,
                Message = "No fingerprints found"
            };
        }

        var probe = new Fingerprint();
        probe.AsBitmap = new System.Drawing.Bitmap(new MemoryStream(dto.ProbeTemplate));

        var probeTemplate = new FingerprintTemplate(probe);

        var matcher = new FingerprintMatcher(probeTemplate);

        double bestScore = 0;
        FingerPrint bestMatch = null;

        foreach (var fp in allFingerprints)
        {
            var candidate = new Fingerprint();
            candidate.AsBitmap = new System.Drawing.Bitmap(
                new MemoryStream(fp.FingerPrintEncoding));

            var candidateTemplate = new FingerprintTemplate(candidate);

            double score = matcher.Match(candidateTemplate);

            if (score > bestScore)
            {
                bestScore = score;
                bestMatch = fp;
            }
        }

        // 🔥 Threshold tuning (important)
        if (bestScore >= 40 && bestMatch != null)
        {
            return new FingerPrintResponseModel
            {
                Status = true,
                Message = $"Fingerprint matched (score: {bestScore})",
                Data = new GetFingerPrintDto
                {
                    Id = bestMatch.Id,
                    PersonId = bestMatch.PersonId,
                    FingerPrintId = bestMatch.FingerPrintId
                }
            };
        }

        return new FingerPrintResponseModel
        {
            Status = false,
            Message = $"No match found (best score: {bestScore})"
        };
    }
}