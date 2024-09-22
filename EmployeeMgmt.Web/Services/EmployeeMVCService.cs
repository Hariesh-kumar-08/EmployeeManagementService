using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EmployeeMgmt.Web.Models;
using Microsoft.Extensions.Configuration;
using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Web.Services;

public class EmployeeMVCService : IEmployeeMVCService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmployeeMVCService> _logger;
    private readonly ITokenService _tokenService;

    public EmployeeMVCService(HttpClient httpClient, IConfiguration configuration, ILogger<EmployeeMVCService> logger, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _tokenService = tokenService;
    }

    // Add token to headers
    private async Task AddTokenToHeaderAsync()
    {
        string token = _tokenService.GenerateServiceToken();

        // Log the token to ensure it’s generated
        _logger.LogInformation("Generated Token: " + token);

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Log the headers to verify they are correctly set
            _logger.LogInformation("Authorization Header Set: " + _httpClient.DefaultRequestHeaders.Authorization);
        }
        else
        {
            _logger.LogError("Failed to generate a valid JWT token");
        }
    }

    public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
    {
        try
        {
            _logger.LogInformation("GetAllEmployeesAsync: EmployeeService - Fetching all employees");
            _logger.LogInformation("Request Headers: " + string.Join(", ", _httpClient.DefaultRequestHeaders.Select(h => $"{h.Key}: {h.Value}")));
           // await AddTokenToHeaderAsync();
            string token = _tokenService.GenerateServiceToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_configuration["ApiSettings:BaseUrl"]}employee");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<EmployeeDTO>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in GetAllEmployeesAsync: EmployeeService due to " + ex.Message);
            throw;
        }
    }

    public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("GetEmployeeByIdAsync: EmployeeService - Fetching employee with ID {Id}", id);

            await AddTokenToHeaderAsync();

            var response = await _httpClient.GetAsync($"{_configuration["ApiSettings:BaseUrl"]}employee/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EmployeeDTO>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in GetEmployeeByIdAsync: EmployeeService due to " + ex.Message);
            throw;
        }
    }

    public async Task AddEmployeeAsync(EmployeeDTO employeeDto)
    {
        try
        {
            _logger.LogInformation("AddEmployeeAsync: EmployeeService - Adding new employee");

            await AddTokenToHeaderAsync();

            var jsonContent = JsonConvert.SerializeObject(employeeDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["ApiSettings:BaseUrl"]}employee", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in AddEmployeeAsync: EmployeeService due to " + ex.Message);
            throw;
        }
    }

    public async Task UpdateEmployeeAsync(EmployeeDTO employeeDto)
    {
        try
        {
            _logger.LogInformation("UpdateEmployeeAsync: EmployeeService - Updating employee with ID {Id}", employeeDto.EmployeeId);

            await AddTokenToHeaderAsync();

            var jsonContent = JsonConvert.SerializeObject(employeeDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_configuration["ApiSettings:BaseUrl"]}employee/{employeeDto.EmployeeId}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in UpdateEmployeeAsync: EmployeeService due to " + ex.Message);
            throw;
        }
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        try
        {
            _logger.LogInformation("DeleteEmployeeAsync: EmployeeService - Deleting employee with ID {Id}", id);

            await AddTokenToHeaderAsync();

            var response = await _httpClient.DeleteAsync($"{_configuration["ApiSettings:BaseUrl"]}employee/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in DeleteEmployeeAsync: EmployeeService due to " + ex.Message);
            throw;
        }
    }
}
