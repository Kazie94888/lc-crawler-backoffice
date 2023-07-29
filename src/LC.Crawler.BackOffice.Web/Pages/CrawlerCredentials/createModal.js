var abp = abp || {};

abp.modals.crawlerCredentialCreate = function () {
    var initModal = function (publicApi, args) {
        var l = abp.localization.getResource("BackOffice");
        
        
        var lastNpIdId = '';
        var lastNpDisplayNameId = '';

        var _lookupModal = new abp.ModalManager({
            viewUrl: abp.appPath + "Shared/LookupModal",
            scriptUrl: "/Pages/Shared/lookupModal.js",
            modalClass: "navigationPropertyLookup"
        });

        $('.lookupCleanButton').on('click', '', function () {
            $(this).parent().find('input').val('');
        });

        _lookupModal.onClose(function () {
            var modal = $(_lookupModal.getModal());
            $('#' + lastNpIdId).val(modal.find('#CurrentLookupId').val());
            $('#' + lastNpDisplayNameId).val(modal.find('#CurrentLookupDisplayName').val());
        });
        
        $('#CrawlerAccountLookupOpenButton').on('click', '', function () {
            lastNpDisplayNameId = 'CrawlerAccount_Username';
            lastNpIdId = 'CrawlerAccount_Id';
            _lookupModal.open({
                currentId: $('#CrawlerAccount_Id').val(),
                currentDisplayName: $('#CrawlerAccount_Username').val(),
                serviceMethod: function() {
                    
                    return window.lC.crawler.backOffice.crawlerCredentials.crawlerCredentials.getCrawlerAccountLookup;
                }
            });
        });
        $('#CrawlerProxyLookupOpenButton').on('click', '', function () {
            lastNpDisplayNameId = 'CrawlerProxy_Ip';
            lastNpIdId = 'CrawlerProxy_Id';
            _lookupModal.open({
                currentId: $('#CrawlerProxy_Id').val(),
                currentDisplayName: $('#CrawlerProxy_Ip').val(),
                serviceMethod: function() {
                    
                    return window.lC.crawler.backOffice.crawlerCredentials.crawlerCredentials.getCrawlerProxyLookup;
                }
            });
        });
        
        
    };

    return {
        initModal: initModal
    };
};