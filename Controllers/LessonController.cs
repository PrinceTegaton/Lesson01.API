using Lesson01.API.DTO;
using Lesson01.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lesson01.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LessonController : ControllerBase
    {
        private readonly IExamServiceAdapter _examServiceAdapter;
        private readonly IPaymentGatewayAdapter _paymentGatewayAdapter;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<LessonController> _logger;

        public LessonController(IExamServiceAdapter examServiceAdapter,
                                IPaymentGatewayAdapter paymentGatewayAdapter,
                                IServiceScopeFactory serviceScopeFactory,
                                ILogger<LessonController> logger)
        {
            _examServiceAdapter = examServiceAdapter;
            _paymentGatewayAdapter = paymentGatewayAdapter;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        /// <summary>
        /// Get student exam scores
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="examCode"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<ExamScoreModel>>), 200)]
        public async Task<IActionResult> GetExamScore(string studentId, string examCode)
        {
            var scoreRes = await _examServiceAdapter.GetScores(studentId, examCode);

            if (!scoreRes.Succeeded || scoreRes.Data == null || !scoreRes.Data.Any())
            {
                return BadRequest("Exam score not found");
            }

            return Ok(scoreRes.Data);
        }

        /// <summary>
        /// Pay for lesson fees
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="examCode"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<string>), 200)]
        public async Task<IActionResult> PayFees([Required] CardPaymentModel model)
        {
            // validate input

            // validate business logic

            // process payment
            var payRes = await _paymentGatewayAdapter.PayWithCard(model);

            if (!payRes.Succeeded || string.IsNullOrEmpty(payRes.Data))
            {
                return BadRequest($"Payment failed - {payRes?.Message}");
            }

            // update record with payment reference


            // send email in background
            var scope = _serviceScopeFactory.CreateScope();
            var emailAdapter = scope.ServiceProvider.GetService<IEmailAdapter>();

            Action action = new(async () =>
            {
                await emailAdapter.SendEmail("student@email.com", "Successful Payment", EmailTemplate.PaymentSuccessful,
                                    new Dictionary<string, string>
                                    {
                                        { EmailParameters.Name, "Student Name" },
                                        { EmailParameters.Amount, $"${model.Amount:N2}" }
                                    });
            });

            new Task(action, CancellationToken.None, TaskCreationOptions.LongRunning).Start();

            return Ok(payRes);
        }
    }
}