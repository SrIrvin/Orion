using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;

namespace Orión.Infrastructure.Services;

public class ReportService : IReportService
{
    public ReportService()
    {
        // El tipo correcto es LicenseType, no LicenseKind
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public void GenerateSolicitudPdf(SolicitudServicioDto solicitud, string filePath)
    {
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("GESTOR ORIÓN").FontSize(24).Bold().FontColor("#003153");
                        col.Item().Text("ORDEN DE SERVICIO DE MANTENIMIENTO").FontSize(12).SemiBold().FontColor("#4682B4");
                    });

                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item().Text($"FOLIO: SS-{solicitud.IdSS:D5}").FontSize(14).Bold();
                        col.Item().Text($"FECHA: {solicitud.FechaApertura:dd/MM/yyyy}").FontSize(10);
                    });
                });

                page.Content().PaddingVertical(20).Column(col =>
                {
                    // Sección Información
                    col.Item().BorderBottom(1).PaddingBottom(5).Text("INFORMACIÓN GENERAL").SemiBold();
                    
                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().Text(x => { x.Span("MAQUINARIA: ").Bold(); x.Span(solicitud.NombreMaquinaria); });
                        table.Cell().Text(x => { x.Span("ID MÁQUINA: ").Bold(); x.Span(solicitud.IdMaquinaria); });
                        
                        table.Cell().Text(x => { x.Span("TIPO MTTO: ").Bold(); x.Span(solicitud.TipoMantenimientoDescripcion); });
                        table.Cell().Text(x => { x.Span("ESTADO: ").Bold(); x.Span(solicitud.EstadoDescripcion); });
                        
                        table.Cell().ColumnSpan(2).Text(x => { x.Span("TÉCNICO: ").Bold(); x.Span(solicitud.TecnicoNombre); });
                    });

                    // Sección Falla
                    col.Item().PaddingTop(20).Border(1).Padding(10).Column(innerCol =>
                    {
                        innerCol.Item().Text("DESCRIPCIÓN DE LA FALLA / TRABAJO REALIZADO").Bold().FontSize(11);
                        innerCol.Item().PaddingTop(5).Text(solicitud.DescripcionFalla);
                    });

                    // Espacio para firmas
                    col.Item().PaddingTop(50).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().BorderTop(1).AlignCenter().Text("FIRMA TÉCNICO");
                        });
                        row.ConstantItem(50);
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().BorderTop(1).AlignCenter().Text("FIRMA SUPERVISOR");
                        });
                    });
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf(filePath);
    }
}
