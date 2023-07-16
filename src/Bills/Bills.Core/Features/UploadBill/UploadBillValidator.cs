using FluentValidation;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.Features.UploadBill;

internal sealed class UploadBillValidator : AbstractValidator<UploadBillCommand>
{
	public UploadBillValidator()
	{
        RuleFor(command => command.File).MustAsync(async (file, cancellationToken) =>
        {
            try
            {
                await Image.DetectFormatAsync(file, cancellationToken);
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                        ex is NotSupportedException ||
                                        ex is InvalidImageContentException ||
                                        ex is UnknownImageFormatException)
            {
                return false;
            }

            return true;
        }).WithMessage("Stream does not contain valid image file");
	}
}
