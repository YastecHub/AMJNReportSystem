﻿using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class QuestionController : BaseSecuredController
	{
		private readonly IQuestionService _questionService;
		public QuestionController(IQuestionService questionService)
        {
			_questionService = questionService;

		}

		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPost("Create Question")]
		[OpenApiOperation("Create new question.", "")]
		public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest model, [FromServices] IValidator<CreateQuestionRequest> validator)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.ToDictionary());
			var questionRequest = await _questionService.CreateQuestion(model);
			return !questionRequest.Succeeded ? Conflict(questionRequest) : Ok(questionRequest);
		}

		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPut("Update Question")]
		[OpenApiOperation("Update question.", "")]
		public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest model ,Guid id, [FromServices] IValidator<UpdateQuestionRequest> validator)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.ToDictionary());
			var reportTypeRequest = await _questionService.UpdateQuestion(id,model);
			return !reportTypeRequest.Succeeded ? Conflict(reportTypeRequest) : Ok(reportTypeRequest);
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)] 
		[HttpGet("{id}")]
		[OpenApiOperation("Get a specific question by id.", "")]
		public async Task<IActionResult> GetQuestion(Guid id)
		{
			if (id == Guid.Empty) 
				return BadRequest("id can not be empty");
			var response = await _questionService.GetQuestion(id);
			return !response.Succeeded ? NotFound(response) : Ok(response);
		}

		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpGet]
		[OpenApiOperation("Get list of all question.", "")]
		public async Task<IActionResult> GetReportTypes() 
		{
			var response = await _questionService.GetQuestions();
			return Ok(response);
		}

		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpGet("qaidReportTypes")]
		[OpenApiOperation("Get list of all question by section", "")]
		public async Task<IActionResult> GetQuestionsBySection(Guid sectionId)
		{
			var qaidReportType = await _questionService.GetQuestionsBySection(sectionId);
			return Ok(qaidReportType);
		}
	}
}
