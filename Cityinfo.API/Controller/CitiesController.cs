using AutoMapper;
using Cityinfo.API.Models;
using Cityinfo.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cityinfo.API.Controller
{
    [ApiController]
    [Authorize]
    [Route("api/Cities")]
    public class CitiesController : ControllerBase

    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;


        public CitiesController(ICityInfoRepository cityInfoRepository,IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities()
        {
            var Cities = await _cityInfoRepository.GetCitiesAsync();
            return Ok(
                _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(Cities)
                );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>
            Getcity(int id, bool includePointOfInterest = false)

        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointOfInterest)
            {

                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
        }
    }

}
