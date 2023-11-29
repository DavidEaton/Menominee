using FluentValidation;
using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;
using System;
using System.Linq;

namespace Menominee.Api.Features.Contactables.Emails
{
    public class EmailsRequestValidator : ItemsWithPrimaryValidator<EmailToWrite, EmailRequestValidator>
    {
        private readonly ApplicationDbContext context;
        public EmailsRequestValidator(ApplicationDbContext context) : base()
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(emails => emails)
                .ForEach(emailRule =>
                {
                    emailRule
                        .NotEmpty()
                        .WithMessage(Email.EmptyMessage);
                });

            //RuleFor(emails => emails)
            //    .ForEach(emailRule =>
            //    {
            //        emailRule
            //            .Must(email =>
            //                email.Id == 0 || EmailIsUnique(email))
            //            .WithMessage(Email.DuplicateMessage);
            //    });
        }

        private bool EmailIsUnique(EmailToWrite emailRequest)
        {
            return !context.Emails.Any(email => email.Address == emailRequest.Address);
        }
    }
}
