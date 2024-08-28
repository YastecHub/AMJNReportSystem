using AMJNReportSystem.Application.Abstractions;
using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.RequestModels.Reports;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Services
{
    public class ReportService : IReportService
    {
       // private readonly IReportRepository _reportRepository;
        private readonly IReportTypeSectionRepository _typeSectionRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IReportDataSectionRepository _dataSectionRepository;
        public ReportService(IReportTypeSectionRepository typeSectionRepository, IEncryptionService encryptionService, IQuestionRepository questionRepository, IReportDataSectionRepository reportDataSectionRepository)
        {
           // _reportRepository = reportRepository;
            _typeSectionRepository = typeSectionRepository;
            _encryptionService = encryptionService;
            _questionRepository = questionRepository;
            _dataSectionRepository = reportDataSectionRepository;
        }
        public async Task<Result<bool>> SaveReport(ReportRequest request)
        {
            //// TODO: check the submission window if it is not locked
            //// TODO: check the submission window for report submission timeliness
            //// TODO: check the submission status of the report

            //// TODO: Check if report already created
            //// TODO: check if report has not been submitted already.....return if true
            //var report = new Report
            //{
            //    ReporterId = request.ReporterId,
            //    SubmissionWindowId = request.SubmissionWindowId,
            //    ReportSubmissionTimeliness = ReportSubmissionTimeliness.Early,
            //    ReportSubmissionStatus = ReportSubmissionStatus.Inprogress,
            //    // TODO get the current login user to get the user submitting the report
            //    CreatedBy = "",
            //};
            //await _reportRepository.AddAsync(report);


            //foreach (var section in request.SectionReports)
            //{
            //    // Check if there are any duplicate report questions
            //    if (section.SectionData.DistinctBy(q => q?.QuestionId ?? Guid.Empty).Count() != section.SectionData.Count)
            //    {
            //        throw new ArgumentException("Duplicate report questions are not allowed.");
            //    }
            //    var encryptedReportSectionData = section.SectionData.Select(question => new
            //    {
            //        question.QuestionText,
            //        QuestionResponse = _encryptionService.Encrypt(question.QuestionResponse),
            //        // TODO : to check if the question has response, then apply the score to it or any further business logic
            //    }).ToList();

            //    var reportData = JsonConvert.SerializeObject(encryptedReportSectionData);

            //    var dataSection = await _dataSectionRepository.GetReportDataSection(x => x.ReportId == report.Id && x.ReportTypeSectionId == section.ReportTypeSectionId);

            //    if (dataSection is null)
            //    {
            //        var reportDataSection = new ReportSection
            //        {
            //            ReportId = report.Id,
            //            Report = report,
            //            ReportTypeSectionId = section.ReportTypeSectionId,
            //            ReportSectionName = section.ReportTypeSectionName,
            //            Data = reportData
            //        };
            //        await _dataSectionRepository.AddAsync(reportDataSection);
            //    }
            //    else
            //    {
            //        dataSection.Data = reportData;
            //        await _dataSectionRepository.UpdateAsync(dataSection);
            //    }
            //}
            return await Result<bool>.SuccessAsync("Report Successfully Updated");
        }
    }
}
