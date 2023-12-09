using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using System.Data;
namespace Home_Security.Implementations.Services;
public class PersonService : IPersonService
{
    IPersonRepo _personRepo;
    IFingerPrintRepo _fingerPrintRepo;
    IUserRepo _userRepo;
    IAddressRepo _addressRepo;
    IContactDetailsRepo _contactDetailsRepo;
    public PersonService(IPersonRepo personRepo, IFingerPrintRepo fingerPrintRepo, IUserRepo userRepo, IAddressRepo addressRepo, IContactDetailsRepo contactDetailsRepo)
    {
        _personRepo = personRepo;
        _fingerPrintRepo = fingerPrintRepo;
        _userRepo = userRepo;
        _addressRepo = addressRepo;
        _contactDetailsRepo = contactDetailsRepo;
    }
    public async Task<BaseResponse> CreatePerson(CreatePersonDto createPersonDto)
    {
        var checkUser = _userRepo.Get(x => x.UserName == createPersonDto.CreateUserDto.UserName);
        if (checkUser == null )
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "..\\Images\\Persons\\");
            if (!System.IO.Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var imagePath = "";
            if (createPersonDto.CreatePersonDetailsDto.ImageUrl != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(createPersonDto.CreatePersonDetailsDto.ImageUrl.FileName);
                var filePath = Path.Combine(folderPath, createPersonDto.CreatePersonDetailsDto.ImageUrl.FileName);
                var extension = Path.GetExtension(createPersonDto.CreatePersonDetailsDto.ImageUrl.FileName);
                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await createPersonDto.CreatePersonDetailsDto.ImageUrl.CopyToAsync(stream);
                    }
                    imagePath = fileName;
                }
            }
            var user = new User()
            {
                UserName = createPersonDto.CreateUserDto.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(createPersonDto.CreateUserDto.Password),
                AuthorizationCode = BCrypt.Net.BCrypt.EnhancedHashPassword(createPersonDto.CreateUserDto.AuthorizationCode),
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
                PersonDetails = new PersonDetails()
                {
                    FirstName = createPersonDto.CreatePersonDetailsDto.FirstName,
                    LastName = createPersonDto.CreatePersonDetailsDto.LastName,
                    DateOfBirth = createPersonDto.CreatePersonDetailsDto.DateOfBirth,
                    Age = createPersonDto.CreatePersonDetailsDto.Age,
                    Gender = createPersonDto.CreatePersonDetailsDto.Gender,
                    ImageUrl = imagePath,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                }
            };
            var getPerson = await _personRepo.Create(person);
            List<FingerPrint> Fingerprints = new List<FingerPrint>();
            foreach (var fingerprint in createPersonDto.CreatePersonDetailsDto.CreateFingerPrintDtos)
            {
                Fingerprints.Add(new FingerPrint()
                {
                    PersonId = getPerson.Id,
                    FingerPrintId = $"FINGER{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                    FingerPrintEncoding = fingerprint.FingerPrintEncoding.ToString(),
                    IsDeleted = false,
                    LastModifiedBy = getPerson.Id,
                    LastModifiedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    CreatedBy = getPerson.Id
                });
            }
            getPerson.User.PersonId = getPerson.Id;
            person.PersonDetails.FingerPrints = Fingerprints;
            await _personRepo.Update(person);
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
        if (person != null) {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "..\\Images\\Persons\\");
            if (!System.IO.Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var imagePath = "";
            if (updatePersonDto.UpdatePersonDetailsDto.ImageUrl != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(updatePersonDto.UpdatePersonDetailsDto.ImageUrl.FileName);
                var filePath = Path.Combine(folderPath, updatePersonDto.UpdatePersonDetailsDto.ImageUrl.FileName);
                var extension = Path.GetExtension(updatePersonDto.UpdatePersonDetailsDto.ImageUrl.FileName);
                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await updatePersonDto.UpdatePersonDetailsDto.ImageUrl.CopyToAsync(stream);
                    }
                    imagePath = fileName;
                }
            }
            person.User.UserName = updatePersonDto.UpdateUserDto.UserName ?? person.User.UserName;
            person.User.UserRole.Role = updatePersonDto.UpdateUserDto.Role;
            person.PersonDetails.FirstName = updatePersonDto.UpdatePersonDetailsDto.FirstName ?? person.PersonDetails.FirstName;
            person.PersonDetails.LastName = updatePersonDto.UpdatePersonDetailsDto.LastName ?? person.PersonDetails.LastName;
            person.PersonDetails.ImageUrl = imagePath ?? person.PersonDetails.ImageUrl;
            person.PersonDetails.Gender = updatePersonDto.UpdatePersonDetailsDto.Gender;
            person.LastModifiedOn = DateTime.Now;
            foreach (var contactDetail in updatePersonDto.UpdatePersonDetailsDto.GetContactDetailsDtos)
            {
                var contactDetailes = await _contactDetailsRepo.Get(x => x.Id == contactDetail.Id && x.IsDeleted == false);
                if (contactDetailes != null)
                {
                    contactDetailes.PhoneNumber = contactDetail.PhoneNumber;
                    contactDetailes.Email = contactDetail.Email;
                    contactDetailes.LastModifiedBy = updatePersonDto.Id;
                    contactDetailes.LastModifiedOn = DateTime.Now;
                    await _contactDetailsRepo.Update(contactDetailes);
                };
            }
            foreach (var addresses in updatePersonDto.UpdatePersonDetailsDto.UpdateAddressDtos)
            {
                var address = await _addressRepo.Get(x => x.Id == addresses.Id && x.IsDeleted == false);
                if (address != null)
                {
                    address.NumberLine = addresses.NumberLine;
                    address.Street = addresses.Street;
                    address.City = addresses.City;
                    address.Region = addresses.Region;
                    address.State = addresses.State;
                    address.Country = addresses.Country;
                    address.PostalCode = addresses.PostalCode;
                    address.LastModifiedBy = updatePersonDto.Id;
                    address.LastModifiedOn = DateTime.Now;
                    await _addressRepo.Update(address);
                }
            }
            await _personRepo.Update(person);
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
    public async Task<BaseResponse> AddPersonAddress(int personId, List<CreateAddressDto> createAddressDtos)
    {
        var person = await _personRepo.GetById(personId);
        if (person != null)
        {
            foreach (var addresses in createAddressDtos)
            {
                var address = new Address()
                {
                    PersonDetailsId = person.PersonDetails.Id,
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
                var contactDetailes = new ContactDetails()
                {
                    PersonDetailsId = person.PersonDetails.Id,
                    PhoneNumber = contactDetail.PhoneNumber,
                    Email = contactDetail.Email,
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
    public async Task<BaseResponse> DisablePerson(int id)
    {
        var person = await _personRepo.GetById(id);
        if(person != null)
        {
            if (person.Disabled == true) person.Disabled = false;
            else person.Disabled = true;
            await _personRepo.Update(person);
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
                ImageUrl = person.PersonDetails.ImageUrl,
                Gender = person.PersonDetails.Gender,
                GetContactDetailsDtos = person.PersonDetails.ContactDetails.Select(x => GetContactDetails(x)).ToList(),
                GetAddressDtos = person.PersonDetails.Addresses.Select(x => GetAddressDetails(x)).ToList(),
            }
        };
    }
    public GetAddressDto GetAddressDetails(Address address)
    {
        return new GetAddressDto()
        {
            Id = address.Id,
            ContactId = address.ContactId,
            PersonId = address.PersonDetailsId,
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
            ContactId = contactDetails.ContactId,
            PersonDetailsId = contactDetails.PersonDetailsId,
            PhoneNumber = contactDetails.PhoneNumber,
            Email = contactDetails.Email,
        };
    }
}