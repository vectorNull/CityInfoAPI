using System.Text.Json;
using AutoMapper;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            [FromQuery] string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }

        /// <summary>
        /// Get a city by Id
        /// </summary>
        /// <param name="id">The Id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include points of interest</param>
        /// <return>An IActionResult</return>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
           var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

           if (city == null)
           {
                return NotFound();
           }

           if (includePointsOfInterest)
           {
                return Ok(_mapper.Map<CityDto>(city));
           }

           return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}