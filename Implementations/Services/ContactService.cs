using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;
namespace Home_Security.Implementations.Services;
public class ContactService : IContactService
{
    IContactRepo _contactRepo;
    IAddressRepo _addressRepo;
    IContactDetailsRepo _contactDetailsRepo;
    IRealtimeNotificationService _realTime;
    public ContactService(IContactRepo contactRepo, IAddressRepo addressRepo, IContactDetailsRepo contactDetailsRepo, IRealtimeNotificationService realTime)
    {
        _contactRepo = contactRepo;
        _addressRepo = addressRepo;
        _contactDetailsRepo = contactDetailsRepo;
        _realTime = realTime;
    }
    public async Task<BaseResponse> CreateContact(CreateContactDto createContactDto)
    {
        if(createContactDto != null) {
            Console.WriteLine("Contact Category: " + createContactDto.ContactCategory);
            var contact = new Contact()
            {
                ContactId = $"CONTACT{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                FirstName = createContactDto.FirstName,
                LastName = createContactDto.LastName,
                ContactCategoryId = createContactDto.ContactCategory,
                Gender = createContactDto.Gender,
                ImageUrl = await GetImageUrl(createContactDto.ImageUrl),
            };
            contact = await _contactRepo.Create(contact);
            foreach (var contactDetail in createContactDto.ContactDetails)
            {
                var contactDetailes = new ContactDetails()
                {
                    ContactId = contact.Id,
                    PhoneNumber = contactDetail.PhoneNumber,
                    Email = contactDetail.Email,
                    IsDeleted = false,
                    CreatedBy = createContactDto.personId,
                    LastModifiedBy = createContactDto.personId,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };
                contactDetailes = await _contactDetailsRepo.Create(contactDetailes);
                contact.ContactDetails.Add(contactDetailes);
            }
            foreach (var addresses in createContactDto.Address)
            {
                var address = new Address()
                {
                    ContactId = contact.Id,
                    NumberLine = addresses.NumberLine,
                    Street = addresses.Street,
                    City = addresses.City,
                    Region = addresses.Region,
                    State = addresses.State,
                    Country = addresses.Country,
                    PostalCode = addresses.PostalCode,
                    IsDeleted = false,
                    CreatedBy = createContactDto.personId,
                    LastModifiedBy = createContactDto.personId,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };
                address = await _addressRepo.Create(address);
                contact.Address.Add(address);
            }
            await _realTime.NotifyAll("ContactCreated", GetDetails(await _contactRepo.GetById(contact.Id)));
            return new BaseResponse()
            {
                Message = "Contact Added Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Message = "Unable To Add Contact!",
            Status = false
        };
    }
    public async Task<BaseResponse> UpdateContact(UpdateContactDto updateContactDto)
    {
        var contact = await _contactRepo.GetById(updateContactDto.Id);
        if (contact != null)
        {
            contact.FirstName = updateContactDto.FirstName ?? contact.FirstName;
            contact.LastName = updateContactDto.LastName ?? contact.LastName;
            contact.ImageUrl = await GetImageUrl(updateContactDto.ImageUrl) ?? contact.ImageUrl;
            contact.Gender = updateContactDto.Gender;
            contact.LastModifiedOn = DateTime.Now;
            contact.LastModifiedBy = updateContactDto.personId;
            contact.ContactCategoryId = updateContactDto.ContactCategory;
            foreach (var contactDetail in updateContactDto.ContactDetails)
            {
                if(contactDetail.Id == 0)
                {
                    var contactDetailes = new ContactDetails()
                    {
                        ContactId = contact.Id,
                        PersonDetailsId = null,
                        PhoneNumber = contactDetail.PhoneNumber,
                        Email = contactDetail.Email,
                        IsDeleted = false,
                        CreatedBy = updateContactDto.personId,
                        LastModifiedBy = updateContactDto.personId,
                        CreatedOn = DateTime.Now,
                        LastModifiedOn = DateTime.Now,
                    };
                    contactDetailes = await _contactDetailsRepo.Create(contactDetailes);
                    contact.ContactDetails.Add(contactDetailes);
                }
                else
                {
                    var contactDetailes = await _contactDetailsRepo.Get(x => x.Id == contactDetail.Id && x.IsDeleted == false);
                    if(contactDetailes != null)
                    {
                        contactDetailes.PhoneNumber = contactDetail.PhoneNumber;
                        contactDetailes.Email = contactDetail.Email;
                        contactDetailes.LastModifiedBy = updateContactDto.personId;
                        contactDetailes.LastModifiedOn = DateTime.Now;
                        await _contactDetailsRepo.Update(contactDetailes);
                    };
                }
            }
            foreach (var addresses in updateContactDto.Address)
            {
                if(addresses.Id == 0)
                {
                    var address = new Address()
                    {
                        PersonDetailsId = null,
                        ContactId = contact.Id,
                        NumberLine = addresses.NumberLine,
                        Street = addresses.Street,
                        City = addresses.City,
                        Region = addresses.Region,
                        State = addresses.State,
                        Country = addresses.Country,
                        PostalCode = addresses.PostalCode,
                        IsDeleted = false,
                        CreatedBy = updateContactDto.personId,
                        LastModifiedBy = updateContactDto.personId,
                        CreatedOn = DateTime.Now,
                        LastModifiedOn = DateTime.Now,
                    };
                    address = await _addressRepo.Create(address);
                    contact.Address.Add(address);
                }
                else
                {
                    var address = await _addressRepo.Get(x => x.Id == addresses.Id && x.IsDeleted == false);
                    if(address != null)
                    {
                        address.NumberLine = addresses.NumberLine;
                        address.Street = addresses.Street;
                        address.City = addresses.City;
                        address.Region = addresses.Region;
                        address.State = addresses.State;
                        address.Country = addresses.Country;
                        address.PostalCode = addresses.PostalCode;
                        address.LastModifiedBy = updateContactDto.personId;
                        address.LastModifiedOn = DateTime.Now;
                        address = await _addressRepo.Update(address);
                    }
                }
            }
            await _contactRepo.Update(contact);
            await _realTime.NotifyAll("ContactUpdated", GetDetails(contact));
            return new BaseResponse()
            {
                Status = true,
                Message = "Contact Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Contact!"
        };
    }
    public async Task<ContactResponseModel> GetContactById(int id)
    {
        var contact = await _contactRepo.GetById(id);
        if(contact != null)
        {
            return new ContactResponseModel()
            {
                Data = GetDetails(contact),
                Message = "Contact Retrieved Successfully!",
                Status = true
            };
        }
        return new ContactResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Contact!"
        };
    }
    public async Task<ContactsResponseModel> GetContactByFirstName(string firstName)
    {
        var contacts = await _contactRepo.GetByFirstName(firstName);
        if (contacts != null)
        {
            return new ContactsResponseModel()
            {
                Data = contacts.Select(x => GetDetails(x)).ToList(),
                Message = "Contacts Retrieved Successfully!",
                Status = true
            };
        }
        return new ContactsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Contacts!"
        };
    }
    public async Task<ContactsResponseModel> GetContactByLastName(string lastName)
    {
        var contacts = await _contactRepo.GetByFirstName(lastName);
        if (contacts != null)
        {
            return new ContactsResponseModel()
            {
                Data = contacts.Select(x => GetDetails(x)).ToList(),
                Message = "Contacts Retrieved Successfully!",
                Status = true
            };
        }
        return new ContactsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Contacts!"
        };
    }
    public async Task<ContactsResponseModel> GetContactByContactCategory(int id)
    {
        var contacts = await _contactRepo.GetByContactCategory(id);
        if (contacts != null)
        {
            return new ContactsResponseModel()
            {
                Data = contacts.Select(x => GetDetails(x)).ToList(),
                Message = "Contacts Retrieved Successfully!",
                Status = true
            };
        }
        return new ContactsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Contacts!"
        };
    }
    public async Task<ContactsResponseModel> GetAllContacts()
    {
        var contacts = await _contactRepo.List();
        if (contacts != null)
        {
            return new ContactsResponseModel()
            {
                Data = contacts.Select(x => GetDetails(x)).ToList(),
                Message = "Contacts Retrieved Successfully!",
                Status = true
            };
        }
        return new ContactsResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Contacts!"
        };
    }
    public async Task<BaseResponse> Delete(int id, int personId)
    {
        var contact = await _contactRepo.GetById(id);
        if (contact != null)
        {
            contact.IsDeleted = true;
            foreach (var contactDetail in contact.ContactDetails)
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
            foreach (var addresses in contact.Address)
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
            await _contactRepo.Update(contact);
            await _realTime.NotifyAll("ContactDeleted", new {
                Id = contact.Id,
                IsDeleted = contact.IsDeleted
            });
            return new BaseResponse()
            {
                Message = "Contact Deleted Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Contact!"
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
            await _realTime.NotifyAll("ContactAddressDeleted", new {
                Id = contactDetail.Id,
                IsDeleted = contactDetail.IsDeleted
            });
            return new BaseResponse()
            {
                Message = "Contact Details Deleted Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Contact Details!"
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
            await _realTime.NotifyAll("ContactAddressDeleted", new {
                Id = address.Id,
                IsDeleted = address.IsDeleted
            });
            return new BaseResponse()
            {
                Message = "Contact Detail Deleted Successfully!",
                Status = true
            };
        };
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Contact Detail!"
        };
    }
    public async Task<string> GetImageUrl(string file){
        if (file == null || file.Length == 0)
            return null;
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
        var folderName = Path.Combine("Contact", "Images");
        var folderPath = Path.Combine(storagePath, folderName);
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
        var absolutePath = Path.Combine(folderPath, uniqueFileName);
        await File.WriteAllBytesAsync(absolutePath, bytes);
        return Path.Combine(folderName, uniqueFileName);
    }
    public GetContactDto GetDetails(Contact contact)
    {
        return new GetContactDto()
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            ImageUrl = contact.ImageUrl,
            Gender = contact.Gender,
            GetContactCategoryDto = new GetContactCategoryDto()
            {
                Id = contact.Category.Id,
                Name = contact.Category.Name,
                Description = contact.Category.Description,
            } ?? new GetContactCategoryDto(),
            ContactDetails = contact.ContactDetails.Select(x => GetContactDetails(x)).ToList(),
            Address = contact.Address.Select(x => GetAddressDetails(x)).ToList(),
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
}