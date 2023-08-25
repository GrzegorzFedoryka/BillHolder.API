using FluentValidation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Core.Functions.Features.UploadBill;

internal sealed class UploadBillValidator : AbstractValidator<UploadBillCommand>
{
    public UploadBillValidator()
    {
        RuleFor(command => command).MustAsync(async (command, cancellationToken) =>
        {
            try
            {
                var format = await Image.DetectFormatAsync(command.File, cancellationToken);
                command.Format = format;
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
