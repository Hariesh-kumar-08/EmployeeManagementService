using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Web.Services;

public class DepartmentMVCService : IDepartmentMVCService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DepartmentMVCService> _logger;
    private readonly ITokenService _tokenService;

    public DepartmentMVCService(HttpClient httpClient, IConfiguration configuration, ILogger<DepartmentMVCService> logger, ITokenService tokenService)
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
        _logger.LogInformation("Generated Token: " + token);

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _logger.LogInformation("Authorization Header Set: " + _httpClient.DefaultRequestHeaders.Authorization);
        }
        else
        {
            _logger.LogError("Failed to generate a valid JWT token");
        }
    }

    public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
    {
        try
        {
            _logger.LogInformation("GetAllDepartmentsAsync: Fetching all departments");
            await AddTokenToHeaderAsync();

            var response = await _httpClient.GetAsync($"{_configuration["ApiSettings:BaseUrl"]}department");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DepartmentDTO>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in GetAllDepartmentsAsync: " + ex.Message);
            throw;
        }
    }

    public async Task<DepartmentDTO> GetDepartmentByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("GetDepartmentByIdAsync: Fetching department with ID {Id}", id);
            await AddTokenToHeaderAsync();

            var response = await _httpClient.GetAsync($"{_configuration["ApiSettings:BaseUrl"]}department/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DepartmentDTO>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in GetDepartmentByIdAsync: " + ex.Message);
            throw;
        }
    }

    public async Task AddDepartmentAsync(DepartmentDTO departmentDto)
    {
        try
        {
            _logger.LogInformation("AddDepartmentAsync: Adding new department");
            await AddTokenToHeaderAsync();

            var jsonContent = JsonConvert.SerializeObject(departmentDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["ApiSettings:BaseUrl"]}department", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in AddDepartmentAsync: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateDepartmentAsync(DepartmentDTO departmentDto)
    {
        try
        {
            _logger.LogInformation("UpdateDepartmentAsync: Updating department with ID {Id}", departmentDto.DepartmentId);
            await AddTokenToHeaderAsync();

            var jsonContent = JsonConvert.SerializeObject(departmentDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_configuration["ApiSettings:BaseUrl"]}department/{departmentDto.DepartmentId}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in UpdateDepartmentAsync: " + ex.Message);
            throw;
        }
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        try
        {
            _logger.LogInformation("DeleteDepartmentAsync: Deleting department with ID {Id}", id);
            await AddTokenToHeaderAsync();

            var response = await _httpClient.DeleteAsync($"{_configuration["ApiSettings:BaseUrl"]}department/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in DeleteDepartmentAsync: " + ex.Message);
            throw;
        }
    }
}
