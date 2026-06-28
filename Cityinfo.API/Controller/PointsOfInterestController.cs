using AutoMapper;
using Cityinfo.API.Entities;
using Cityinfo.API.Models;
using Cityinfo.API.Repository;
using Cityinfo.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Cityinfo.API.Controller
{
    // /api/cities/3/PointsOfinterest
    [Route("api/cities/{cityId}/PointsOfinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;


        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;


        private readonly CitiesDataStore citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> loger,
            IMailService localMailService, CitiesDataStore citiesDataStore
            , ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = loger ?? throw new ArgumentNullException(nameof(loger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));

            _cityInfoRepository = cityInfoRepository ??
                 throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            this.citiesDataStore = citiesDataStore;


        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>>
            GetPointsOfInterest(int cityId)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"{cityId} Not Found...");
                return NotFound();
            }
            var pointsOfInterestForCity = await _cityInfoRepository
                 .GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }
        [HttpGet("{PointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId, int PointOfInterestId
            )
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var PointOfInterest = await _cityInfoRepository
                 .GetPointsOfInterestForCityAsync(cityId, PointOfInterestId);

            if (PointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PointOfInterestDto>(PointOfInterest));

        }
        //#region post

        [HttpPost]

        public async Task<ActionResult<PointOfInterestDto>> CreatPointOfInterest(
           int cityId,
           PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPoint = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            await _cityInfoRepository.AddPointOfInterestForCityAsync(
                cityId, finalPoint);
            await _cityInfoRepository.SaveChangesAsync();

            var createdpoint = _mapper.Map<Models.PointOfInterestDto>(finalPoint);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                PointOfInterestId = createdpoint.Id

            }, createdpoint);
        }
        //    #endregion


        #region Edit

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId,
            int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var point = await _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId, pointOfInterestId);
            if (point == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, point);

            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();

        }
        #endregion

        #region Edit With Patch
        [HttpPatch("{pointOfInterestid}")]

        public async Task<ActionResult> partiallyUpdatePointOfInterest(
           int cityId,
            int pointOfInterestid,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument
            )


        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointEntity = await _cityInfoRepository
                 .GetPointsOfInterestForCityAsync(cityId, pointOfInterestid);
            if (pointEntity == null)
            {
                NotFound();
            }

            var pointToPatch = _mapper.Map<PointOfInterestForUpdateDto>
                (pointEntity);

            patchDocument.ApplyTo(pointToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(pointToPatch, pointEntity);
            await _cityInfoRepository.SaveChangesAsync();


            return NoContent();
        }
        #endregion


        #region Delete
        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(
            int cityId,
            int pointOfInterestId)

        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity =
                await _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                NotFound();
            }
            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();


            _localMailService
                .Send(
                "Point Of interest delete",
                $"Point Of Interest {pointOfInterestEntity.Name}with id {pointOfInterestEntity.Id}"
                );

            return NoContent();
        }
        #endregion
    }
}
