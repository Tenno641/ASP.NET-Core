using CsvHelper;
using Entities;
using Entities.DataAccess;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RepositoryContracts;
using Services.Helpers;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;

namespace Services.Persons;
public class PersonsService : IPersonsService
{
    private readonly IPersonsRepository _repository;
    private readonly PersonsDbContext _dbContext;
    public PersonsService(IPersonsRepository repository, PersonsDbContext dbContext)
    {
        _dbContext = dbContext;
        _repository = repository;
    }
    public async Task<PersonResponse> AddPersonAsync(PersonRequest? personRequest)
    {
        ArgumentNullException.ThrowIfNull(personRequest);
        (bool isValid, IReadOnlyCollection<ValidationResult> validationResults) objectValidation = ValidationHelper.ValidateObject(personRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.validationResults.Select(result => result.ErrorMessage)));

        Person person = personRequest.ToPerson();
        person.Id = Guid.NewGuid();

        await _repository.AddPersonAsync(person);
        //InsertPersonStoredProcedure(person);

        return PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);
    }
    public async Task<PersonResponse?> GetAsync(Guid? id)
    {
        if (id is null) return null;

        Person? person = await _repository.GetAsync(id.Value);
        if (person is null) return null;

        return PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);
    }
    public async Task<IEnumerable<PersonResponse>> GetAllAsync()
    {
        return await _repository.All().Select(PersonResponseExtension.ToPersonResponse).ToListAsync();
    }
    public async Task<IEnumerable<PersonResponse>> FilterAsync(string searchBy, string? searchString)
    {
        if (searchString is null) return await GetAllAsync();
        if (!int.TryParse(searchString, out var age))
            return await GetAllAsync(); 

        return searchBy switch
        {
            nameof(PersonResponse.Name) => await _repository.FilterAsync(person => person.Name == null || person.Name.Contains(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),

            nameof(PersonResponse.Email) => await _repository.FilterAsync(person => person.Email == null || person.Email.Contains(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),

            nameof(PersonResponse.DateOfBirth) => await _repository.FilterAsync(person => person.DateOfBirth == null || person.DateOfBirth.Value.ToString("yyyy-mm-ddd").Contains(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),

            nameof(PersonResponse.Gender) => await _repository.FilterAsync(person => person.Gender == null || person.Gender.Equals(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),

            nameof(PersonResponse.CountryName) => await _repository.FilterAsync(person => person.Country == null || person.Country.Name == null || person.Country.Name.Contains(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),

            nameof(PersonResponse.Address) => await _repository.FilterAsync(person => person.Address == null || person.Address.Contains(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),

            nameof(PersonResponse.ReceiveNewsLetter) => await _repository.FilterAsync(person => person.ReceiveNewsLetter.Equals(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),

            nameof(PersonResponse.Age) => await _repository.FilterAsync(person => person.DateOfBirth == null || EF.Functions.DateDiffYear(person.DateOfBirth.Value, DateTime.UtcNow) == age).Select(PersonResponseExtension.ToPersonResponse).Where(personResponse => personResponse.Age == age).ToListAsync(),

            _ => await GetAllAsync()
        };
    }
    private async Task<IEnumerable<PersonResponse>> OrderGenericAsync<T>(IEnumerable<PersonResponse> data, Func<PersonResponse, T> selector, SortOrderOptions sortOrderOptions)
    {
        return sortOrderOptions switch
        {
            SortOrderOptions.Descending => data.OrderByDescending(selector).ToList(),
            SortOrderOptions.Ascending => data.OrderBy(selector).ToList(),
            _ => await GetAllAsync()
        };
    }
    public async Task<IEnumerable<PersonResponse>> OrderAsync(IEnumerable<PersonResponse> data, string sortBy, SortOrderOptions sortOptions)
    {
        return sortBy switch
        {
            "Name" => await OrderGenericAsync(data, person => person.Name, sortOptions),
            "Email" => await OrderGenericAsync(data, person => person.Email, sortOptions),
            "DateOfBirth" => await OrderGenericAsync(data, person => person.DateOfBirth, sortOptions),
            "Age" => await OrderGenericAsync(data, person => person.Age, sortOptions),
            "Gender" => await OrderGenericAsync(data, person => person.Gender, sortOptions),
            "Country" => await OrderGenericAsync(data, person => person.CountryName, sortOptions),
            "Address" => await OrderGenericAsync(data, person => person.Address, sortOptions),
            "ReceiveNewsLetters" => await OrderGenericAsync(data, person => person.ReceiveNewsLetter, sortOptions),
            _ => await GetAllAsync()
        };
    }
    public async Task<PersonResponse> UpdateAsync(PersonUpdateRequest? personUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        var objectValidation = ValidationHelper.ValidateObject(personUpdateRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.errors.Select(error => error.ErrorMessage)));

        Person? person = await _repository.GetAsync(personUpdateRequest.Id);

        if (person is null) throw new ArgumentException("Not Found Person");

        await _repository.UpdateAsync(personUpdateRequest.ToPerson());

        return PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);
    }

    public async Task<bool> DeleteAsync(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Person? person = await _repository.GetAsync(id.Value);
        if (person is null) return false;

        return await _repository.RemoveAsync(id.Value);
    }


    public async Task<MemoryStream> GetPersonsCsvAsync()
    {
        MemoryStream memoryStream = new MemoryStream();
        StreamWriter streamWriter = new StreamWriter(memoryStream);
        CsvWriter csvWriter = new CsvWriter(streamWriter, culture: CultureInfo.InvariantCulture, leaveOpen: true);
        csvWriter.WriteHeader<PersonResponse>();

        await csvWriter.NextRecordAsync();

        IEnumerable<PersonResponse> persons = await GetAllAsync();
        await csvWriter.WriteRecordsAsync<PersonResponse>(persons);
        await csvWriter.FlushAsync();
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<MemoryStream> GetPersonsExcelAsync()
    {
        MemoryStream memoryStream = new();
        ExcelPackage.License.SetNonCommercialPersonal("Tenno641");
        using ExcelPackage excelPackage = new ExcelPackage(memoryStream);

        ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add("Persons");

        sheet.Cells["A1"].Value = "ID";
        sheet.Cells["B1"].Value = "Name";
        sheet.Cells["C1"].Value = "Email";
        sheet.Cells["D1"].Value = "Date Of Birth";

        using ExcelRange sheetHeaders = sheet.Cells["A1:D1"];
        sheetHeaders.Style.Fill.SetBackground(Color.AntiqueWhite);
        sheetHeaders.Style.Font.Bold = true;
        sheetHeaders.Style.Fill.PatternType = ExcelFillStyle.DarkVertical;
        int row = 2;
        IEnumerable<PersonResponse> persons = await GetAllAsync();

        foreach (PersonResponse person in persons)
        {
            sheet.Cells[row, 1].Value = person.Id;
            sheet.Cells[row, 2].Value = person.Name;
            sheet.Cells[row, 3].Value = person.Email;
            sheet.Cells[row, 4].Value = person.DateOfBirth?.ToString("yyyy-mm-dd");
            row++;
        }
        sheet.Cells["A1:D5"].AutoFitColumns();
        await excelPackage.SaveAsync();
        memoryStream.Position = 0;
        return memoryStream;
    }
}
