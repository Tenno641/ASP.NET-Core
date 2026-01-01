using CsvHelper;
using Entities;
using Entities.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Services.Helpers;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Services.Persons;
public class PersonsService : IPersonsService
{
    private readonly PersonsDbContext _dbContext;
    public PersonsService(PersonsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PersonResponse> AddPersonAsync(PersonRequest? personRequest)
    {
        ArgumentNullException.ThrowIfNull(personRequest);
        (bool isValid, IReadOnlyCollection<ValidationResult> validationResults) objectValidation = ValidationHelper.ValidateObject(personRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.validationResults.Select(result => result.ErrorMessage)));

        Person person = personRequest.ToPerson();
        person.Id = Guid.NewGuid();

        _dbContext.Persons.Add(person);
        await _dbContext.SaveChangesAsync();
        //InsertPersonStoredProcedure(person);

        return PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);
    }
    public async Task<PersonResponse?> GetAsync(Guid? id)
    {
        if (id is null) return null;

        return await _dbContext.Persons
            .Include(person => person.Country)
            .AsNoTracking()
            .Where(person => person.Id == id)
            .Select(PersonResponseExtension.ToPersonResponse)
            .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<PersonResponse>> GetAllAsync()
    {
        return await _dbContext.Persons.Include(person => person.Country).Select(PersonResponseExtension.ToPersonResponse).ToListAsync();
    }
    public async Task<IEnumerable<PersonResponse>> FilterAsync(string searchBy, string? searchString)
    {
        if (searchString is null) return await GetAllAsync();

        return searchBy switch
        {
            "Name" => await _dbContext.Persons.Where(person => person.Name == null || person.Name.Contains(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),
            "Email" => await _dbContext.Persons.Where(person => person.Email == null || person.Email.Contains(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),
            "DateOfBirth" => await _dbContext.Persons.Where(person => person.DateOfBirth == null || person.DateOfBirth.Value.ToString("yyyy-mm-ddd").Contains(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),
            "Age" => await _dbContext.Persons.Select(PersonResponseExtension.ToPersonResponse).Where(person => person.Age.Equals(searchString)).ToListAsync(),
            "Gender" => await _dbContext.Persons.Where(person => person.Gender == null || person.Gender.Equals(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),
            "Country" => await _dbContext.Persons.Include(person => person.Country).Select(PersonResponseExtension.ToPersonResponse).Where(person => person.CountryName == null || person.CountryName.Equals(searchString)).ToListAsync(),
            "Address" => await _dbContext.Persons.Where(person => person.Address == null || person.Address.Equals(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),
            "ReceiveNewsLetters" => await _dbContext.Persons.Where(person => person.ReceiveNewsLetter.Equals(searchString)).Select(PersonResponseExtension.ToPersonResponse).ToListAsync(),
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

        Person? person = _dbContext.Persons.FirstOrDefault(person => person.Id == personUpdateRequest.Id);

        if (person is null) throw new ArgumentException("Not Found Person");

        UpdatePerson(personUpdateRequest, person);
        await _dbContext.SaveChangesAsync();

        return PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);
    }
    public async Task<bool> DeleteAsync(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Person? person = await _dbContext.Persons.FirstOrDefaultAsync(person => person.Id == id);
        if (person is null) return false;

        _dbContext.Remove(person);
        return await _dbContext.SaveChangesAsync() > 0;
    }
    public IEnumerable<Person> GetAllPersonsStoredProcedure()
    {
        FormattableString query = FormattableStringFactory.Create("EXECUTE [dbo].[PersonsGet]");
        return _dbContext.Persons.FromSql(query);
    }
    public void InsertPersonStoredProcedure(Person person)
    {
        SqlParameter[] sqlParameters = new SqlParameter[]
        {
            new("@Id", person.Id),
            new("@Name", person.Name),
            new("@Email", person.Email),
            new("@DateOfBirth", person.DateOfBirth),
            new("@Gender", person.Gender),
            new("@CountryId", person.CountryId),
            new("@Address", person.Address),
            new("@ReceiveNewsLetter", person.ReceiveNewsLetter)
        };
        _dbContext.Database.ExecuteSqlRaw("EXECUTE [dbo].[PersonInsert] @Id, @Name, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetter", sqlParameters);
    }
    private static void UpdatePerson(PersonUpdateRequest personUpdateRequest, Person person)
    {
        person.Address = personUpdateRequest.Address ?? person.Address;
        person.CountryId = personUpdateRequest.CountryId ?? person.CountryId;
        person.DateOfBirth = personUpdateRequest.DateOfBirth ?? person.DateOfBirth;
        person.Email = personUpdateRequest.Email ?? person.Email;
        person.Gender = personUpdateRequest.Gender.ToString() ?? person.Gender;
        person.Name = personUpdateRequest.Name ?? person.Name;
        person.ReceiveNewsLetter = personUpdateRequest.ReceiveNewsLetter;
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
