using System.Net.Http.Json;
using Microsoft.JSInterop;
using PPDIS.Shared.Models;
using Radzen.Blazor;
using Soil.Shared.Models.InventoryModel;

namespace Soil.Client.Pages.Analytics;

public partial class Analytics
{
    private RadzenDataGrid<PurchaseRequest> grid;
    private List<PurchaseRequest> items = new();
    private List<string> trackingNumbers = new();
    private string searchTerm = "";
    private string selectedTracking = "";

    // Show nothing until a tracking number is selected
    private IEnumerable<PurchaseRequest> filteredItems => string.IsNullOrWhiteSpace(selectedTracking)
        ? new List<PurchaseRequest>()
        : items.Where(i =>
            (string.IsNullOrWhiteSpace(searchTerm) || i.PRNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) &&
            i.TrackingNo == selectedTracking
        );

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
        await LoadQuotations();
        await LoadPOs();
        await LoadInspections();
    }

    private async Task LoadData()
    {
        items = await Http.GetFromJsonAsync<List<PurchaseRequest>>("api/inventory/get_pr");

        // Extract unique tracking numbers for dropdown
        trackingNumbers = items.Select(i => i.TrackingNo)
                               .Distinct()
                               .OrderBy(x => x)
                               .ToList();
    }

    private void FilterItems(object value)
    {
        searchTerm = value?.ToString() ?? "";
        grid.Reload();
    }

    private void FilterByTracking(object value)
    {
        selectedTracking = value?.ToString() ?? "";
        grid.Reload();
    }

    private void ClearFilters()
    {
        searchTerm = "";
        selectedTracking = "";
        grid.Reload();
    }

    private async Task OnUpdateRow(PurchaseRequest item)
    {
        await Http.PutAsJsonAsync($"api/inventory/{item.Id}", item);
    }

    private async Task OnDeleteRow(PurchaseRequest item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            await Http.DeleteAsync($"api/inventory/{item.Id}");
        }
    }
    
    
    private async Task PrintFirstStockCard(PurchaseRequest item)
    {
        if (item == null) return;

        var query = $"?id={Uri.EscapeDataString(item.Id ?? "")}" +
                    $"&propertyNo={Uri.EscapeDataString(item.TrackingNo ?? "")}" +  $"&propertyId={Uri.EscapeDataString(item.PrId ?? "")}" ;

        await JS.InvokeVoidAsync("open", $"/api/Report/StockReport{query}", "_blank");
    }

    private async Task PrintAllStockCard()
    {
        var report_stocks = await Http.GetFromJsonAsync<List<PurchaseRequest>>("api/inventory/get_pr");
        if (report_stocks == null || !report_stocks.Any()) return;

        // ✅ If a drop-down value is selected, filter by it (take first match only)
        if (!string.IsNullOrEmpty(selectedTracking))
        {
            var selectedStock = report_stocks.FirstOrDefault(x => x.TrackingNo == selectedTracking);
            if (selectedStock == null) return;

            var query = $"id={Uri.EscapeDataString(selectedStock.Id ?? "")}" +
                        $"&propertyNo={Uri.EscapeDataString(selectedStock.TrackingNo ?? "")}" +
                        $"&propertyId={Uri.EscapeDataString(selectedStock.PrId ?? "")}";

            await JS.InvokeVoidAsync("open", $"/api/Report/StockReportAll?{query}", "_blank");
        }
        else
        {
            // ✅ If nothing selected, fallback to all
            var query = string.Join("&", report_stocks.Select(x =>
                $"id={Uri.EscapeDataString(x.Id ?? "")}" +
                $"&propertyNo={Uri.EscapeDataString(x.TrackingNo ?? "")}" +
                $"&propertyId={Uri.EscapeDataString(x.PrId ?? "")}"
            ));

            await JS.InvokeVoidAsync("open", $"/api/Report/StockReportAll?{query}", "_blank");
        }
    }

    
    
    //---------------------------------------------------------------------- QUOTATION ----------------------------------------------------------------------//
    
    
    
    

    private RadzenDataGrid<QuotationClass> quotationGrid;
    private List<QuotationClass> quotations = new();
    private List<string> quotationNumbers = new();
    private string searchTermQuotations = "";
    private string selectedQuotation = "";

    // Show nothing until a quotation number is selected
    private IEnumerable<QuotationClass> filteredQuotations => string.IsNullOrWhiteSpace(selectedQuotation)
        ? new List<QuotationClass>()
        : quotations.Where(q =>
            (string.IsNullOrWhiteSpace(searchTermQuotations) || q.PRNumber.Contains(searchTermQuotations, StringComparison.OrdinalIgnoreCase)) &&
            q.TrackingNo == selectedQuotation
        );

  

    private async Task LoadQuotations()
    {
        quotations = await Http.GetFromJsonAsync<List<QuotationClass>>("api/inventory/get_qu");

        // Extract unique quotation numbers (CanvassNo) for dropdown
        quotationNumbers = quotations.Select(q => q.TrackingNo)
                                     .Distinct()
                                     .OrderBy(x => x)
                                     .ToList();
    }

    private void FilterQuotations(object value)
    {
        searchTermQuotations = value?.ToString() ?? "";
        quotationGrid.Reload();
    }

    private void FilterByQuotation(object value)
    {
        selectedQuotation = value?.ToString() ?? "";
        quotationGrid.Reload();
    }

    private void ClearQuotationFilters()
    {
        searchTermQuotations = "";
        selectedQuotation = "";
        quotationGrid.Reload();
    }

    private async Task UpdateQuotationRow(QuotationClass quotation)
    {
        await Http.PutAsJsonAsync($"api/inventory/{quotation.Id}", quotation);
    }

    private async Task DeleteQuotationRow(QuotationClass quotation)
    {
        if (quotations.Contains(quotation))
        {
            quotations.Remove(quotation);
            await Http.DeleteAsync($"api/inventory/{quotation.Id}");
        }
    }

    private async Task PrintQuotation(QuotationClass quotation)
    {
        if (quotation == null) return;

        var query = $"?id={Uri.EscapeDataString(quotation.Id.ToString() ?? "")}" +
                    $"&propertyNo={Uri.EscapeDataString(quotation.TrackingNo ?? "")}" +
                    $"&propertyId={Uri.EscapeDataString(quotation.PRNumber ?? "")}";

        await JS.InvokeVoidAsync("open", $"/api/Report/QuotationReport{query}", "_blank");
    }


    private async Task PrintAllQuotations()
    {
        var reportQuotations = await Http.GetFromJsonAsync<List<QuotationClass>>("api/inventory/get_qu");
        if (reportQuotations == null || !reportQuotations.Any()) return;

        if (!string.IsNullOrEmpty(selectedQuotation))
        {
            var selected = reportQuotations.FirstOrDefault(x => x.CanvassNo == selectedQuotation);
            if (selected == null) return;

            var query = $"id={Uri.EscapeDataString(selected.Id.ToString())}" +
                        $"&prNumber={Uri.EscapeDataString(selected.PRNumber ?? "")}" +
                        $"&canvassNo={Uri.EscapeDataString(selected.CanvassNo ?? "")}";

            await JS.InvokeVoidAsync("open", $"/api/Report/QuotationReportAll?{query}", "_blank");
        }
        else
        {
            var query = string.Join("&", reportQuotations.Select(x =>
                $"id={Uri.EscapeDataString(x.Id.ToString())}" +
                $"&prNumber={Uri.EscapeDataString(x.PRNumber ?? "")}" +
                $"&canvassNo={Uri.EscapeDataString(x.CanvassNo ?? "")}"
            ));

            await JS.InvokeVoidAsync("open", $"/api/Report/QuotationReportAll?{query}", "_blank");
        }
    }
    
    
    
    
    //---------------------------------------------------------------------- PURCHASE ORDER ----------------------------------------------------------------------//
    
    
    
      private RadzenDataGrid<PurchaseOrderClass> poGrid;
    private List<PurchaseOrderClass> purchaseOrders = new();
    private List<string> poNumbers = new();
    private string searchTermPo = "";
    private string selectedPO = "";

    private IEnumerable<PurchaseOrderClass> filteredPOs => string.IsNullOrWhiteSpace(selectedPO)
        ? new List<PurchaseOrderClass>()
        : purchaseOrders.Where(po =>
            (string.IsNullOrWhiteSpace(searchTermPo) || po.PONumber.Contains(searchTermPo, StringComparison.OrdinalIgnoreCase)) &&
            po.TrackingNo == selectedPO
        );


    private async Task LoadPOs()
    {
        purchaseOrders = await Http.GetFromJsonAsync<List<PurchaseOrderClass>>("api/inventory/get_po");

        poNumbers = purchaseOrders.Select(po => po.TrackingNo)
                                  .Distinct()
                                  .OrderBy(x => x)
                                  .ToList();
    }

    private void FilterPOs(object value)
    {
        searchTermPo = value?.ToString() ?? "";
        poGrid.Reload();
    }

    private void FilterByPO(object value)
    {
        selectedPO = value?.ToString() ?? "";
        poGrid.Reload();
    }

    private void ClearPOFilters()
    {
        searchTermPo = "";
        selectedPO = "";
        poGrid.Reload();
    }

    private async Task UpdatePORow(PurchaseOrderClass po)
    {
        await Http.PutAsJsonAsync($"api/inventory/{po.Id}", po);
    }

    private async Task DeletePORow(PurchaseOrderClass po)
    {
        if (purchaseOrders.Contains(po))
        {
            purchaseOrders.Remove(po);
            await Http.DeleteAsync($"api/inventory/{po.Id}");
        }
    }

    private async Task PrintPO(PurchaseOrderClass po)
    {
        if (po == null) return;

        var query = $"?id={Uri.EscapeDataString(po.Id.ToString())}" +
                    $"&poNumber={Uri.EscapeDataString(po.PONumber ?? "")}";

        await JS.InvokeVoidAsync("open", $"/api/Report/POReport{query}", "_blank");
    }

    private async Task PrintAllPOs()
    {
        var allPOs = await Http.GetFromJsonAsync<List<PurchaseOrderClass>>("api/inventory/get_po");
        if (allPOs == null || !allPOs.Any()) return;

        var query = string.Join("&", allPOs.Select(x =>
            $"id={Uri.EscapeDataString(x.Id.ToString())}" +
            $"&poNumber={Uri.EscapeDataString(x.PONumber ?? "")}"
        ));

        await JS.InvokeVoidAsync("open", $"/api/Report/POReportAll?{query}", "_blank");
    }
    
    
    
    //---------------------------------------------------------------------- INSPECTION ----------------------------------------------------------------------//
    
    
    
    
     private RadzenDataGrid<InspectionClass> inspectionGrid;
    private List<InspectionClass> inspections = new();
    private List<string> inspectionNumbers = new();
    private string searchTermIns = "";
    private string selectedInspection = "";

    private IEnumerable<InspectionClass> filteredInspections => string.IsNullOrWhiteSpace(selectedInspection)
        ? new List<InspectionClass>()
        : inspections.Where(i =>
            (string.IsNullOrWhiteSpace(searchTermIns) || i.InspectionNumber.Contains(searchTermIns, StringComparison.OrdinalIgnoreCase)) &&
            i.TrackingNo == selectedInspection
        );

   

    private async Task LoadInspections()
    {
        inspections = await Http.GetFromJsonAsync<List<InspectionClass>>("api/inventory/get_ins");

        inspectionNumbers = inspections.Select(i => i.TrackingNo)
                                       .Distinct()
                                       .OrderBy(x => x)
                                       .ToList();
    }

    private void FilterInspections(object value)
    {
        searchTermIns = value?.ToString() ?? "";
        inspectionGrid.Reload();
    }

    private void FilterByInspection(object value)
    {
        selectedInspection = value?.ToString() ?? "";
        inspectionGrid.Reload();
    }

    private void ClearInspectionFilters()
    {
        searchTermIns = "";
        selectedInspection = "";
        inspectionGrid.Reload();
    }

    private async Task UpdateInspectionRow(InspectionClass inspection)
    {
        await Http.PutAsJsonAsync($"api/inventory/{inspection.Id}", inspection);
    }

    private async Task DeleteInspectionRow(InspectionClass inspection)
    {
        if (inspections.Contains(inspection))
        {
            inspections.Remove(inspection);
            await Http.DeleteAsync($"api/inventory/{inspection.Id}");
        }
    }

    private async Task PrintInspection(InspectionClass inspection)
    {
        if (inspection == null) return;

        var query = $"?id={Uri.EscapeDataString(inspection.Id.ToString())}" +
                    $"&inspectionNumber={Uri.EscapeDataString(inspection.InspectionNumber ?? "")}";

        await JS.InvokeVoidAsync("open", $"/api/Report/InspectionReport{query}", "_blank");
    }

    private async Task PrintAllInspections()
    {
        var allInspections = await Http.GetFromJsonAsync<List<InspectionClass>>("api/inventory/get_ins");
        if (allInspections == null || !allInspections.Any()) return;

        var query = string.Join("&", allInspections.Select(x =>
            $"id={Uri.EscapeDataString(x.Id.ToString())}" +
            $"&inspectionNumber={Uri.EscapeDataString(x.InspectionNumber ?? "")}"
        ));

        await JS.InvokeVoidAsync("open", $"/api/Report/InspectionReportAll?{query}", "_blank");
    }
    
}


