using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;
using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace Home_Security.Implementations.Services;

public class PersonService : IPersonService
{
    IPersonRepo _personRepo;
    IUserRepo _userRepo;
    IAddressRepo _addressRepo;
    IContactDetailsRepo _contactDetailsRepo;
    IRealtimeNotificationService _realTime;

    public PersonService(IPersonRepo personRepo,IUserRepo userRepo,IAddressRepo addressRepo,IContactDetailsRepo contactDetailsRepo, IRealtimeNotificationService realTime)
    {
        _personRepo = personRepo;
        _userRepo = userRepo;
        _addressRepo = addressRepo;
        _contactDetailsRepo = contactDetailsRepo;
        _realTime = realTime;
    }

    public async Task<BaseResponse> CreatePerson(CreatePersonDto createPersonDto)
    {
        var checkUser = await _userRepo.Get(x => x.UserName == createPersonDto.CreateUserDto.UserName);
        if (checkUser == null)
        {
            var NFID = $"NFID{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16).ToUpper()}";
            var user = new User()
            {
                UserName = createPersonDto.CreateUserDto.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(createPersonDto.CreateUserDto.Password),
                AuthorizationCode = BCrypt.Net.BCrypt.EnhancedHashPassword(createPersonDto.CreateUserDto.AuthorizationCode),
                NFIDCode = NFID,
                NFID = BCrypt.Net.BCrypt.EnhancedHashPassword(NFID),
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                LastModifiedOn = DateTime.Now,
            };
            var getUser = await _userRepo.Create(user);
            var userRole = new UserRole()
            {
                UserId = getUser.Id,
                Role = createPersonDto.CreateUserDto.Role,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                LastModifiedOn = DateTime.Now
            };
            getUser.UserRole = userRole;
            await _userRepo.Update(getUser);
            var person = new Person()
            {
                PersonId = $"PERSON{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                UserId = getUser.Id,
                CameraIds = new List<int>(),
                IsDeleted = false,
                PersonDetails = new PersonDetails()
                {
                    FirstName = createPersonDto.CreatePersonDetailsDto.FirstName,
                    LastName = createPersonDto.CreatePersonDetailsDto.LastName,
                    DateOfBirth = createPersonDto.CreatePersonDetailsDto.DateOfBirth,
                    Age = DateTime.Now.Year - createPersonDto.CreatePersonDetailsDto.DateOfBirth.Year,
                    Gender = createPersonDto.CreatePersonDetailsDto.Gender,
                    ImageUrl = "",
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                },
                CreatedOn = DateTime.Now,
                LastModifiedOn = DateTime.Now,
            };

            await _personRepo.Create(person);
            var personData = await _personRepo.Get(x => x.PersonId == person.PersonId);
            await _realTime.NotifyAll("PersonCreated", GetDetails(await _personRepo.GetById(personData.Id)));
            return new BaseResponse()
            {
                Status = true,
                Message = "Person Added Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Person. Conflicting Username!"
        };
    }

    public async Task<BaseResponse> UpdatePerson(UpdatePersonDto updatePersonDto)
    {
        var person = await _personRepo.GetById(updatePersonDto.Id);
        if (person != null)
        {
            person.User.UserName = updatePersonDto.UpdateUserDto.UserName ?? person.User.UserName;
            person.User.UserRole.Role = updatePersonDto.UpdateUserDto.Role;
            person.CameraIds = updatePersonDto.CameraIds ?? person.CameraIds;
            person.PersonDetails.FirstName = updatePersonDto.UpdatePersonDetailsDto.FirstName ?? person.PersonDetails.FirstName;
            person.PersonDetails.LastName = updatePersonDto.UpdatePersonDetailsDto.LastName ?? person.PersonDetails.LastName;
            person.PersonDetails.ImageUrl = await GetImageUrl(updatePersonDto.UpdatePersonDetailsDto.ImageUrl) ?? person.PersonDetails.ImageUrl;
            person.PersonDetails.Age = DateTime.Now.Year - person.PersonDetails.DateOfBirth.Year;
            person.LastModifiedOn = DateTime.Now;
            await _personRepo.Update(person);
            await _realTime.NotifyAll("PersonUpdated", GetDetails(person));
            return new BaseResponse()
            {
                Status = true,
                Message = "Profile Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Profile!"
        };
    }
    public async Task<BaseResponse> DisablePerson(int id)
    {
        var person = await _personRepo.GetById(id);
        if (person != null)
        {
            person.Disabled = !person.Disabled;
            await _personRepo.Update(person);
            await _realTime.NotifyAll("PersonStatusChanged", new
            {
                person.Id,
                person.Disabled
            });
            return new BaseResponse()
            {
                Status = true,
                Message = "Person Profile Disabled!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Disable Person Profile!"
        };
    }
    public async Task<BaseResponse> AddPersonAddress(int personId, List<CreateAddressDto> createAddressDtos)
    {
        var person = await _personRepo.GetById(personId);
        if (person != null)
        {
            foreach (var addresses in createAddressDtos)
            {
                if (addresses.Id != 0)
                {
                    var address1 = await _addressRepo.Get(x => x.Id == addresses.Id && x.IsDeleted == false);
                    if (address1 != null)
                    {
                        address1.NumberLine = addresses.NumberLine ?? address1.NumberLine;
                        address1.Street = addresses.Street ?? address1.Street;
                        address1.City = addresses.City ?? address1.City;
                        address1.Region = addresses.Region ?? address1.Region;
                        address1.State = addresses.State ?? address1.State;
                        address1.Country = addresses.Country ?? address1.Country;
                        address1.PostalCode = addresses.PostalCode ?? address1.PostalCode;
                        address1.LastModifiedBy = personId;
                        address1.LastModifiedOn = DateTime.Now;
                        await _addressRepo.Update(address1);
                    }
                }
                var address = new Address()
                {
                    PersonDetailsId = person.PersonDetails.Id,
                    ContactId = null,
                    NumberLine = addresses.NumberLine,
                    Street = addresses.Street,
                    City = addresses.City,
                    Region = addresses.Region,
                    State = addresses.State,
                    Country = addresses.Country,
                    PostalCode = addresses.PostalCode,
                    IsDeleted = false,
                    CreatedBy = personId,
                    LastModifiedBy = personId,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };
                await _addressRepo.Create(address);
            }
            return new BaseResponse()
            {
                Message = "Address Added Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Message = "Unable To Add Address!",
            Status = false
        };
    }
    public async Task<BaseResponse> AddPersonContactDetails(int personId, List<CreateContactDetailsDto> createContactDetailsDto)
    {
        var person = await _personRepo.GetById(personId);
        if (person != null)
        {
            foreach (var contactDetail in createContactDetailsDto)
            {
                if (contactDetail.Id != 0)
                {
                    var contactDetailes1 = await _contactDetailsRepo.Get(x => x.Id == contactDetail.Id && x.IsDeleted == false);
                    if (contactDetailes1 != null)
                    {
                        contactDetailes1.PhoneNumber = contactDetail.PhoneNumber ?? contactDetailes1.PhoneNumber;
                        contactDetailes1.Email = contactDetail.Email ?? contactDetailes1.Email;
                        contactDetailes1.LastModifiedBy = personId;
                        contactDetailes1.LastModifiedOn = DateTime.Now;
                        await _contactDetailsRepo.Update(contactDetailes1);
                    }
                }
                var contactDetailes = new ContactDetails()
                {
                    PersonDetailsId = person.PersonDetails.Id,
                    ContactId = null,
                    PhoneNumber = contactDetail.PhoneNumber ?? "",
                    Email = contactDetail.Email ?? "",
                    IsDeleted = false,
                    CreatedBy = personId,
                    LastModifiedBy = personId,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };
                await _contactDetailsRepo.Create(contactDetailes);
            }
            return new BaseResponse()
            {
                Message = "Contact Details Added Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Message = "Unable To Add Contact Details!",
            Status = false
        };
    }
    public async Task<PersonResponseModel> GetPersonById(int id)
    {
        var person = await _personRepo.GetById(id);
        if (person != null)
        {
            return new PersonResponseModel()
            {
                Data = GetDetails(person),
                Status = true,
                Message = "Person Retrieved Successfully!"
            };
        }
        return new PersonResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Rerieve Person!"
        };
    }
    public async Task<PersonsResponseModel> GetAllPersons()
    {
        var persons = await _personRepo.List();
        if(persons != null)
        {
            return new PersonsResponseModel()
            {
                Data = persons.Select(x => GetDetails(x)).ToList(),
                Status = true,
                Message = "Persons Profiles Retrieved Successfully!"
            };
        }
        return new PersonsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Persons Profiles!"
        };
    }
    public async Task<BaseResponse> Delete(int id, int personId)
    {
        var person = await _personRepo.GetById(id);
        if (person != null)
        {
            person.IsDeleted = true;
            foreach (var contactDetail in person.PersonDetails.ContactDetails)
            {

                var contactDetailes = await _contactDetailsRepo.Get(x => x.Id == contactDetail.Id && x.IsDeleted == false);
                if (contactDetailes != null)
                {
                    contactDetail.DeletedBy = personId;
                    contactDetail.DeletedOn = DateTime.Now;
                    contactDetailes.LastModifiedOn = DateTime.Now;
                    contactDetail.IsDeleted = true;
                    await _contactDetailsRepo.Update(contactDetailes);
                };
            }
            foreach (var addresses in person.PersonDetails.Addresses)
            {
                var address = await _addressRepo.Get(x => x.Id == addresses.Id && x.IsDeleted == false);
                if (address != null)
                {
                    address.DeletedBy = personId;
                    address.DeletedOn = DateTime.Now;
                    address.LastModifiedOn = DateTime.Now;
                    address.IsDeleted = true;
                    await _addressRepo.Update(address);
                };
            }
            var user = await _userRepo.Get(x => x.Id == person.UserId);
            if(user != null)
            {
                user.DeletedBy = personId;
                user.IsDeleted = true;
            }
            await _personRepo.Update(person);
            await _realTime.NotifyAll("PersonDeleted", new
            {
                person.Id,
            });
            return new BaseResponse()
            {
                Message = "Person Profile Deleted Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Person Profile!"
        };
    }
    public async Task<BaseResponse> DeleteContactDetails(int id, int personId)
    {
        var contactDetail = await _contactDetailsRepo.Get(x => x.Id == id && x.IsDeleted == false);
        if (contactDetail != null)
        {
            contactDetail.IsDeleted = true;
            contactDetail.DeletedBy = personId;
            contactDetail.DeletedOn = DateTime.Now;
            contactDetail.LastModifiedOn = DateTime.Now;
            contactDetail.IsDeleted = true;
            await _contactDetailsRepo.Update(contactDetail);
            return new BaseResponse()
            {
                Message = "Person Profile Detail Deleted Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Person Profile Detail!"
        };
    }
    public async Task<BaseResponse> DeleteContactAddress(int id, int personId)
    {
        var address = await _addressRepo.Get(x => x.Id == id && x.IsDeleted == false);
        if (address != null)
        {
            address.DeletedBy = personId;
            address.DeletedOn = DateTime.Now;
            address.LastModifiedOn = DateTime.Now;
            address.IsDeleted = true;
            await _addressRepo.Update(address);
            return new BaseResponse()
            {
                Message = "Person Profile Address Deleted Successfully!",
                Status = true
            };
        };

        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Person Profile Address!"
        };
    }
    public GetPersonDto GetDetails(Person person)
    {
        return new GetPersonDto()
        {
            Id = person.Id,
            PersonId = person.PersonId,
            Disabled = person.Disabled,
            CameraIds = person.CameraIds,
            GetUserDto = new GetUserDto()
            {
                Id = person.User.Id,
                UserName = person.User.UserName,
                Role = person.User.UserRole.Role,
                RoleName = person.User.UserRole.Role.ToString()
            },
            GetPersonDetailsDto = new GetPersonDetailsDto()
            {
                Id = person.PersonDetails.Id,
                FirstName = person.PersonDetails.FirstName,
                LastName = person.PersonDetails.LastName,
                ImageUrl = person.PersonDetails.ImageUrl ?? "",
                Gender = person.PersonDetails.Gender,
                GetContactDetailsDtos = person.PersonDetails.ContactDetails.Select(x => GetContactDetails(x)).ToList() ?? new List<GetContactDetailsDto>(),
                GetAddressDtos = person.PersonDetails.Addresses.Select(x => GetAddressDetails(x)).ToList(),
            }
        };
    }
    public GetAddressDto GetAddressDetails(Address address)
    {
        return new GetAddressDto()
        {
            Id = address.Id,
            ContactId = address.ContactId ?? 0,
            PersonId = address.PersonDetailsId ?? 0,
            NumberLine = address.NumberLine,
            Street = address.Street,
            City = address.City,
            Region = address.Region,
            State = address.State,
            Country = address.Country,
            PostalCode = address.PostalCode
        };
    }
    public GetContactDetailsDto GetContactDetails(ContactDetails contactDetails)
    {
        return new GetContactDetailsDto()
        {
            Id = contactDetails.Id,
            ContactId = contactDetails.ContactId ?? 0,
            PhoneNumber = contactDetails.PhoneNumber,
            Email = contactDetails.Email,
        };
    }
    public async Task<string> GetImageUrl(string file){
        if (file == null || file.Length == 0)
            return string.Empty;
        if (string.IsNullOrWhiteSpace(file))
            return null;

        string extension = ".bin";

        if (file.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
        {
            var header = file[..file.IndexOf(',')];
            var mimeType = header.Replace("data:", "").Replace(";base64", "");
            extension = mimeType.ToLower() switch
            {
                "image/jpeg" => ".jpg",
                "image/jpg"  => ".jpg",
                "image/png"  => ".png",
                "image/gif"  => ".gif",
                "image/webp" => ".webp",
                "image/bmp"  => ".bmp",
                "image/tiff" => ".tiff",
                "image/svg+xml" => ".svg",
                _ => ".bin"
            };

            file = file[(file.IndexOf(',') + 1)..];
        }
        byte[] bytes = Convert.FromBase64String(file);
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var storagePath = Path.Combine(basePath, "storage");
        var folderName = Path.Combine("Profile", "Images");
        var folderPath = Path.Combine(storagePath, folderName);
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
        var absolutePath = Path.Combine(folderPath, uniqueFileName);
        await File.WriteAllBytesAsync(absolutePath, bytes);
        return Path.Combine(folderName, uniqueFileName);
    }
}