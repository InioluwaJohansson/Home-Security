using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
using SourceAFIS;

public class FingerprintObject
{
    public byte[] Image { get; set; }

    public IFingerprintTemplate ToTemplate()
    {
        var fingerprint = new Fingerprint();
        fingerprint.AsBitmap = new System.Drawing.Bitmap(new MemoryStream(Image));

        return new FingerprintTemplate(fingerprint);
    }
}
public class FingerPrintService : IFingerPrintService
{
    IFingerPrintRepo _fingerPrintRepo;
    public FingerPrintService(IFingerPrintRepo fingerPrintRepo)
    {
        _fingerPrintRepo = fingerPrintRepo;
    }
    public async Task<FingerPrintResponseModel> VerifyFingerPrint(VerifyFingerPrintDto dto)
    {
        var allFingerprints = await _fingerPrintRepo
            .GetAll(x => x.IsDeleted == false);

        if (allFingerprints == null || !allFingerprints.Any())
        {
            return new FingerPrintResponseModel
            {
                Status = false,
                Message = "No fingerprints found"
            };
        }

        var probe = new Fingerprint();
        probe.AsBitmap = new System.Drawing.Bitmap(new MemoryStream(dto.ProbeTemplate));

        var probeTemplate = new FingerprintTemplate(probe);

        var matcher = new FingerprintMatcher(probeTemplate);

        double bestScore = 0;
        FingerPrint bestMatch = null;

        foreach (var fp in allFingerprints)
        {
            var candidate = new Fingerprint();
            candidate.AsBitmap = new System.Drawing.Bitmap(
                new MemoryStream(fp.FingerPrintEncoding));

            var candidateTemplate = new FingerprintTemplate(candidate);

            double score = matcher.Match(candidateTemplate);

            if (score > bestScore)
            {
                bestScore = score;
                bestMatch = fp;
            }
        }

        // 🔥 Threshold tuning (important)
        if (bestScore >= 40 && bestMatch != null)
        {
            return new FingerPrintResponseModel
            {
                Status = true,
                Message = $"Fingerprint matched (score: {bestScore})",
                Data = new GetFingerPrintDto
                {
                    Id = bestMatch.Id,
                    PersonId = bestMatch.PersonId,
                    FingerPrintId = bestMatch.FingerPrintId
                }
            };
        }

        return new FingerPrintResponseModel
        {
            Status = false,
            Message = $"No match found (best score: {bestScore})"
        };
    }
    public async Task<FingerPrintResponseModel> EnrollFingerPrint(
    EnrollFingerPrintDto dto)
{
    var person = new Person
    {
        Fingerprints =
        {
            new Fingerprint(
                new MemoryStream(dto.FingerPrintImage))
        }
    };

    var template = new FingerprintTemplate(person);

    var entity = new FingerPrint
    {
        PersonId = dto.PersonId,
        FingerPrintEncoding = template.ToByteArray(),
        FingerPrintId = Guid.NewGuid().ToString(),
        IsDeleted = false
    };

    await _fingerPrintRepo.Add(entity);

    return new FingerPrintResponseModel
    {
        Status = true,
        Message = "Fingerprint enrolled successfully",
        Data = new GetFingerPrintDto
        {
            Id = entity.Id,
            PersonId = entity.PersonId,
            FingerPrintId = entity.FingerPrintId
        }
    };
}
}