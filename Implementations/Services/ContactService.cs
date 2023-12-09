using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class ContactService : IContactService
{
    IContactRepo _contactRepo;
    IAddressRepo _addressRepo;
    IContactDetailsRepo _contactDetailsRepo;
    public ContactService(IContactRepo contactRepo, IAddressRepo addressRepo, IContactDetailsRepo contactDetailsRepo)
    {
        _contactRepo = contactRepo;
        _addressRepo = addressRepo;
        _contactDetailsRepo = contactDetailsRepo;
    }
    public async Task<BaseResponse> CreateContact(CreateContactDto createContactDto)
    {
        if(createContactDto != null) {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "..\\Images\\Contacts\\");
            if (!System.IO.Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var imagePath = "";
            if (createContactDto.ImageUrl != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(createContactDto.ImageUrl.FileName);
                var filePath = Path.Combine(folderPath, createContactDto.ImageUrl.FileName);
                var extension = Path.GetExtension(createContactDto.ImageUrl.FileName);
                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await createContactDto.ImageUrl.CopyToAsync(stream);
                    }
                    imagePath = fileName;
                }
            }
            var contact = new Contact()
            {
                ContactId = $"CONTACT{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                FirstName = createContactDto.FirstName,
                LastName = createContactDto.LastName,
                ContactCategory = createContactDto.ContactCategory,
                ImageUrl = imagePath,
            };
            var getContact = await _contactRepo.Create(contact);
            foreach (var contactDetail in createContactDto.ContactDetails)
            {
                var contactDetailes = new ContactDetails()
                {
                    ContactId = getContact.Id,
                    PhoneNumber = contactDetail.PhoneNumber,
                    Email = contactDetail.Email,
                    IsDeleted = false,
                    CreatedBy = createContactDto.personId,
                    LastModifiedBy = createContactDto.personId,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };
                await _contactDetailsRepo.Create(contactDetailes);
            }
            foreach (var addresses in createContactDto.Address)
            {
                var address = new Address()
                {
                    ContactId = getContact.Id,
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
                await _addressRepo.Create(address);
            }
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
    public async Task<BaseResponse> AddContactAddress(int contactId, int personId, List<CreateAddressDto> createAddressDto)
    {
        var contact = await _contactRepo.GetById(contactId);
        if(contact != null)
        {
            foreach (var addresses in createAddressDto)
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
                    CreatedBy = personId,
                    LastModifiedBy = personId,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };
                await _addressRepo.Create(address);
            }
            return new BaseResponse()
            {
                Message = "Contact Address Added Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Message = "Unable To Add Contact Address!",
            Status = false
        };
    }
    public async Task<BaseResponse> AddContactDetails(int contactId, int personId, List<CreateContactDetailsDto> createContactDetailsDto)
    {
        var contact = await _contactRepo.GetById(contactId);
        if (contact != null)
        {
            foreach (var contactDetail in createContactDetailsDto)
            {
                var contactDetailes = new ContactDetails()
                {
                    ContactId = contact.Id,
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
    public async Task<BaseResponse> UpdateContact(UpdateContactDto updateContactDto)
    {
        var contact = await _contactRepo.GetById(updateContactDto.Id);
        if (contact != null)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "..\\Images\\Contacts\\");
            if (!System.IO.Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var imagePath = "";
            if (updateContactDto.ImageUrl != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(updateContactDto.ImageUrl.FileName);
                var filePath = Path.Combine(folderPath, updateContactDto.ImageUrl.FileName);
                var extension = Path.GetExtension(updateContactDto.ImageUrl.FileName);
                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await updateContactDto.ImageUrl.CopyToAsync(stream);
                    }
                    imagePath = fileName;
                }
            }
            contact.FirstName = updateContactDto.FirstName ?? contact.FirstName;
            contact.LastName = updateContactDto.LastName ?? contact.LastName;
            contact.ImageUrl = imagePath ?? contact.ImageUrl;
            contact.LastModifiedOn = DateTime.Now;
            contact.LastModifiedBy = updateContactDto.personId;
            contact.ContactCategory = updateContactDto.ContactCategory;
            foreach (var contactDetail in updateContactDto.ContactDetails)
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
            foreach (var addresses in updateContactDto.Address)
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
                    await _addressRepo.Update(address);
                }
            }
            await _contactRepo.Update(contact);
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
    public GetContactDto GetDetails(Contact contact)
    {
        return new GetContactDto()
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            ImageUrl = contact.ImageUrl,
            GetContactCategoryDto = new GetContactCategoryDto()
            {
                Id = contact.Category.Id,
                Name = contact.Category.Name,
                Description = contact.Category.Description,
                personId = contact.Category.PersonId,
            },
            ContactDetails = contact.ContactDetails.Select(x => GetContactDetails(x)).ToList(),
            Address = contact.Address.Select(x => GetAddressDetails(x)).ToList(),
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