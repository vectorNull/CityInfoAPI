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

        // [HttpGet("{id}")]
        // public ActionResult<CityDto> GetCity(int id)
        // {
        //     var cityToReturn = _citiesDataStore.Cities.Find(c => c.Id == id);

        //     if (cityToReturn == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(cityToReturn);
        // }
    }
}