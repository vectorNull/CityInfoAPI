using AutoMapper;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));

            // var results = new List<CityWithoutPointsOfInterestDto>();
            // foreach (var cityEntity in cityEntities)
            // {
            //     results.Add(new CityWithoutPointsOfInterestDto
            //     {
            //         Id = cityEntity.Id,
            //         Description = cityEntity.Description,
            //         Name = cityEntity.Name
            //     });
            // }

            // return Ok(results);
            // return Ok(_citiesDataStore.Cities);
        }

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