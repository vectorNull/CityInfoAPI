using System.Net.Mail;
using AutoMapper;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with Id {cityId} was not found when accessing points of interest."
                );
                return NotFound();
            }

            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestsForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            [FromRoute] int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var createPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId,
                    pointOfInterestId = finalPointOfInterest.Id
                },
                createPointOfInterestToReturn);
        }

        // [HttpPut("{pointOfInterestId}")]
        // public ActionResult UpdatePointOfInterest(
        //     int cityId,
        //     int pointOfInterestId, 
        //     PointOfInterestForUpdateDto pointOfInterest)
        // {
        //     var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //     if (city == null)
        //     {
        //         return NotFound();
        //     }

        //     var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
        //     if (pointOfInterestFromStore == null)
        //     {
        //         return NotFound();
        //     }

        //     pointOfInterestFromStore.Name = pointOfInterest.Name;
        //     pointOfInterestFromStore.Description = pointOfInterest.Description;

        //     return NoContent();
        // }

        // [HttpPatch("{pointOfInterestId}")]
        // public ActionResult PartiallyUpdatePointOfInterest(
        //     int cityId,
        //     int pointOfInterestId,
        //     JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument
        // )
        // {
        //     var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //     if (city == null)
        //     {
        //         return NotFound();
        //     }

        //     var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
        //     if (pointOfInterestFromStore == null)
        //     {
        //         return NotFound();
        //     }

        //     var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
        //         {
        //             Name = pointOfInterestFromStore.Name,
        //             Description = pointOfInterestFromStore.Description
        //         };

        //     patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     if (!TryValidateModel(pointOfInterestToPatch))
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        //     pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

        //     return NoContent();
        // }

        // [HttpDelete("{pointOfInterestId}")]
        // public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        // {
        //     var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //     if (city == null)
        //     {
        //         return NotFound();
        //     }

        //     var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
        //     if (pointOfInterestFromStore == null)
        //     {
        //         return NotFound();
        //     }

        //     city.PointsOfInterest.Remove(pointOfInterestFromStore);
        //     _mailService.Send("Point of interest was deleted", "Point of interest was delete");

        //     return NoContent();
        // }
    }
}