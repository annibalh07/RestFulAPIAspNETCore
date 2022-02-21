using Microsoft.AspNetCore.Mvc;
using RESTfulAPIExample.Validations;
using System.Net.Mime;

namespace RESTfulAPIExample.Controllers
{
    /// <summary>
    /// Manages patient
    /// </summary>
    [ApiController]
    [Route("[controller]s")]
    public class PatientController : ControllerBase
    {
        #region Data
        private static readonly string[] Names = new[]
        {
            "Leonardo", "Diego", "Roberto", "Manuel", "Karol", "Ericka", "Vladi", "Patricia", "Evelyn", "Dario"
        };

        private static readonly string[] LastNames = new[]
        {
            "Anchundia", "Loor", "Zamora", "Campuzano", "Velez", "Panjón", "Cajas", "Zavala", "Velez", "Fabara"
        };

        private static readonly List<Patient> _patients = Enumerable.Range(1, 5).Select(index => new Patient
        {
            Id = index,
            FirstName = Names[Random.Shared.Next(Names.Length)],
            LastName = LastNames[Random.Shared.Next(LastNames.Length)]
        })
            .ToList();

        #endregion

        private readonly ILogger<PatientController> _logger;

        #region Constructor

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

        #endregion

        /// <summary>
        /// Resturns all the patients
        /// </summary>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        public IEnumerable<Patient> Get()
        {
            return _patients;
        }

        /// <summary>
        /// Retuns a specific patient by id
        /// </summary>
        /// <response code="404">If the patient doesn't exist.</response>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Patient> Get(int id)
        {
            var patient = _patients.FirstOrDefault(t => t.Id == id);

            if (patient == null) return NotFound(new NotFoundInfo { Detail= "Patient doesn't exist." });

            return patient;
        }

        /// <summary>
        /// Creates a new patient
        /// </summary>
        /// <response code="201">If the patient was created successfully.</response>
        /// <response code="400">If One or more validation errors occurred.</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Patient> Save([FromBody] Patient patient)
        {
            _patients.Add(patient);
            return CreatedAtAction(nameof(Get), new { id = patient.Id }, patient);
        }

        /// <summary>
        /// Deletes a specific patient
        /// </summary>
        /// <response code="204">If the patient was deleted successfully.</response>
        /// <response code="404">If the patient was not found.</response>
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Patient> Delete(int id)
        {
            var patient = _patients.FirstOrDefault(t => t.Id == id);

            if (patient == null) return NotFound(new NotFoundInfo { Detail = "Patient doesn't exist." });

            _patients.Remove(patient);

            return NoContent();
        }

        /// <summary>
        /// Updates a specific patient
        /// </summary>
        /// <response code="204">If the patient was updated successfully.</response>
        /// <response code="404">If the patient was not found.</response>
        /// <response code="400">If One or more validation errors occurred.</response>
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Patient> Update(int id, [FromBody] Patient patientRequest)
        {
            var patient = _patients.FirstOrDefault(t => t.Id == id);
            
            if (patient == null) return NotFound(new NotFoundInfo { Detail = "Patient doesn't exist." });

            patient.FirstName = patientRequest.FirstName;
            patient.LastName = patientRequest.LastName;

            return NoContent();
        }
    }
}