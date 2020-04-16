// JavaScript source code
function preFilterLookup() {
    Xrm.Page.getControl("customerid").addPreSearch(function () {
        addLookupFilter();
    });
}
function addLookupFilter() {
    fetchXml = "<filter type='and'> <condition attribute='statecode' operator='eq' value='2' /></filter>";
    Xrm.Page.getControl("customerid").addCustomFilter(fetchXml, "contact");
}