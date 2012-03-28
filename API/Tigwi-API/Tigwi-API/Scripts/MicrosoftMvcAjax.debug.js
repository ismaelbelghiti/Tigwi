//!----------------------------------------------------------
//! Copyright (C) Microsoft Corporation. All rights reserved.
//!----------------------------------------------------------
//! MicrosoftMvcAjax.js

Type.registerNamespace('Sys.Mvc');

////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.AjaxOptions

Sys.Mvc.$create_AjaxOptions = function Sys_Mvc_AjaxOptions() { return {}; }


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.InsertionMode

Sys.Mvc.InsertionMode = function() { 
    /// <field name="replace" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="insertBefore" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="insertAfter" type="Number" integer="true" static="true">
    /// </field>
};
Sys.Mvc.InsertionMode.prototype = {
    replace: 0, 
    insertBefore: 1, 
    insertAfter: 2
}
Sys.Mvc.InsertionMode.registerEnum('Sys.Mvc.InsertionMode', false);


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.AjaxContext

Sys.Mvc.AjaxContext = function Sys_Mvc_AjaxContext(request, updateTarget, loadingElement, insertionMode) {
    /// <param name="request" type="Sys.Net.WebRequest">
    /// </param>
    /// <param name="updateTarget" type="Object" domElement="true">
    /// </param>
    /// <param name="loadingElement" type="Object" domElement="true">
    /// </param>
    /// <param name="insertionMode" type="Sys.Mvc.InsertionMode">
    /// </param>
    /// <field name="_insertionMode" type="Sys.Mvc.InsertionMode">
    /// </field>
    /// <field name="_loadingElement" type="Object" domElement="true">
    /// </field>
    /// <field name="_response" type="Sys.Net.WebRequestExecutor">
    /// </field>
    /// <field name="_request" type="Sys.Net.WebRequest">
    /// </field>
    /// <field name="_updateTarget" type="Object" domElement="true">
    /// </field>
    this._request = request;
    this._updateTarget = updateTarget;
    this._loadingElement = loadingElement;
    this._insertionMode = insertionMode;
}
Sys.Mvc.AjaxContext.prototype = {
    _insertionMode: 0,
    _loadingElement: null,
    _response: null,
    _request: null,
    _updateTarget: null,
    
    get_data: function Sys_Mvc_AjaxContext$get_data() {
        /// <value type="String"></value>
        if (this._response) {
            return this._response.get_responseData();
        }
        else {
            return null;
        }
    },
    
    get_insertionMode: function Sys_Mvc_AjaxContext$get_insertionMode() {
        /// <value type="Sys.Mvc.InsertionMode"></value>
        return this._insertionMode;
    },
    
    get_loadingElement: function Sys_Mvc_AjaxContext$get_loadingElement() {
        /// <value type="Object" domElement="true"></value>
        return this._loadingElement;
    },
    
    get_object: function Sys_Mvc_AjaxContext$get_object() {
        /// <value type="Object"></value>
        var executor = this.get_response();
        return (executor) ? executor.get_object() : null;
    },
    
    get_response: function Sys_Mvc_AjaxContext$get_response() {
        /// <value type="Sys.Net.WebRequestExecutor"></value>
        return this._response;
    },
    set_response: function Sys_Mvc_AjaxContext$set_response(value) {
        /// <value type="Sys.Net.WebRequestExecutor"></value>
        this._response = value;
        return value;
    },
    
    get_request: function Sys_Mvc_AjaxContext$get_request() {
        /// <value type="Sys.Net.WebRequest"></value>
        return this._request;
    },
    
    get_updateTarget: function Sys_Mvc_AjaxContext$get_updateTarget() {
        /// <value type="Object" domElement="true"></value>
        return this._updateTarget;
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.AsyncHyperlink

Sys.Mvc.AsyncHyperlink = function Sys_Mvc_AsyncHyperlink() {
}
Sys.Mvc.AsyncHyperlink.handleClick = function Sys_Mvc_AsyncHyperlink$handleClick(anchor, evt, ajaxOptions) {
    /// <param name="anchor" type="Object" domElement="true">
    /// </param>
    /// <param name="evt" type="Sys.UI.DomEvent">
    /// </param>
    /// <param name="ajaxOptions" type="Sys.Mvc.AjaxOptions">
    /// </param>
    evt.preventDefault();
    Sys.Mvc.MvcHelpers._asyncRequest(anchor.href, 'post', '', anchor, ajaxOptions);
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.MvcHelpers

Sys.Mvc.MvcHelpers = function Sys_Mvc_MvcHelpers() {
}
Sys.Mvc.MvcHelpers._serializeSubmitButton = function Sys_Mvc_MvcHelpers$_serializeSubmitButton(element, offsetX, offsetY) {
    /// <param name="element" type="Object" domElement="true">
    /// </param>
    /// <param name="offsetX" type="Number" integer="true">
    /// </param>
    /// <param name="offsetY" type="Number" integer="true">
    /// </param>
    /// <returns type="String"></returns>
    if (element.disabled) {
        return null;
    }
    var name = element.name;
    if (name) {
        var tagName = element.tagName.toUpperCase();
        var encodedName = encodeURIComponent(name);
        var inputElement = element;
        if (tagName === 'INPUT') {
            var type = inputElement.type;
            if (type === 'submit') {
                return encodedName + '=' + encodeURIComponent(inputElement.value);
            }
            else if (type === 'image') {
                return encodedName + '.x=' + offsetX + '&' + encodedName + '.y=' + offsetY;
            }
        }
        else if ((tagName === 'BUTTON') && (name.length) && (inputElement.type === 'submit')) {
            return encodedName + '=' + encodeURIComponent(inputElement.value);
        }
    }
    return null;
}
Sys.Mvc.MvcHelpers._serializeForm = function Sys_Mvc_MvcHelpers$_serializeForm(form) {
    /// <param name="form" type="Object" domElement="true">
    /// </param>
    /// <returns type="String"></returns>
    var formElements = form.elements;
    var formBody = new Sys.StringBuilder();
    var count = formElements.length;
    for (var i = 0; i < count; i++) {
        var element = formElements[i];
        var name = element.name;
        if (!name || !name.length) {
            continue;
        }
        var tagName = element.tagName.toUpperCase();
        if (tagName === 'INPUT') {
            var inputElement = element;
            var type = inputElement.type;
            if ((type === 'text') || (type === 'password') || (type === 'hidden') || (((type === 'checkbox') || (type === 'radio')) && element.checked)) {
                formBody.append(encodeURIComponent(name));
                formBody.append('=');
                formBody.append(encodeURIComponent(inputElement.value));
                formBody.append('&');
            }
        }
        else if (tagName === 'SELECT') {
            var selectElement = element;
            var optionCount = selectElement.options.length;
            for (var j = 0; j < optionCount; j++) {
                var optionElement = selectElement.options[j];
                if (optionElement.selected) {
                    formBody.append(encodeURIComponent(name));
                    formBody.append('=');
                    formBody.append(encodeURIComponent(optionElement.value));
                    formBody.append('&');
                }
            }
        }
        else if (tagName === 'TEXTAREA') {
            formBody.append(encodeURIComponent(name));
            formBody.append('=');
            formBody.append(encodeURIComponent((element.value)));
            formBody.append('&');
        }
    }
    var additionalInput = form._additionalInput;
    if (additionalInput) {
        formBody.append(additionalInput);
        formBody.append('&');
    }
    return formBody.toString();
}
Sys.Mvc.MvcHelpers._asyncRequest = function Sys_Mvc_MvcHelpers$_asyncRequest(url, verb, body, triggerElement, ajaxOptions) {
    /// <param name="url" type="String">
    /// </param>
    /// <param name="verb" type="String">
    /// </param>
    /// <param name="body" type="String">
    /// </param>
    /// <param name="triggerElement" type="Object" domElement="true">
    /// </param>
    /// <param name="ajaxOptions" type="Sys.Mvc.AjaxOptions">
    /// </param>
    if (ajaxOptions.confirm) {
        if (!confirm(ajaxOptions.confirm)) {
            return;
        }
    }
    if (ajaxOptions.url) {
        url = ajaxOptions.url;
    }
    if (ajaxOptions.httpMethod) {
        verb = ajaxOptions.httpMethod;
    }
    if (body.length > 0 && !body.endsWith('&')) {
        body += '&';
    }
    body += 'X-Requested-With=XMLHttpRequest';
    var upperCaseVerb = verb.toUpperCase();
    var isGetOrPost = (upperCaseVerb === 'GET' || upperCaseVerb === 'POST');
    if (!isGetOrPost) {
        body += '&';
        body += 'X-HTTP-Method-Override=' + upperCaseVerb;
    }
    var requestBody = '';
    if (upperCaseVerb === 'GET' || upperCaseVerb === 'DELETE') {
        if (url.indexOf('?') > -1) {
            if (!url.endsWith('&')) {
                url += '&';
            }
            url += body;
        }
        else {
            url += '?';
            url += body;
        }
    }
    else {
        requestBody = body;
    }
    var request = new Sys.Net.WebRequest();
    request.set_url(url);
    if (isGetOrPost) {
        request.set_httpVerb(verb);
    }
    else {
        request.set_httpVerb('POST');
        request.get_headers()['X-HTTP-Method-Override'] = upperCaseVerb;
    }
    request.set_body(requestBody);
    if (verb.toUpperCase() === 'PUT') {
        request.get_headers()['Content-Type'] = 'application/x-www-form-urlencoded;';
    }
    request.get_headers()['X-Requested-With'] = 'XMLHttpRequest';
    var updateElement = null;
    if (ajaxOptions.updateTargetId) {
        updateElement = $get(ajaxOptions.updateTargetId);
    }
    var loadingElement = null;
    if (ajaxOptions.loadingElementId) {
        loadingElement = $get(ajaxOptions.loadingElementId);
    }
    var ajaxContext = new Sys.Mvc.AjaxContext(request, updateElement, loadingElement, ajaxOptions.insertionMode);
    var continueRequest = true;
    if (ajaxOptions.onBegin) {
        continueRequest = ajaxOptions.onBegin(ajaxContext) !== false;
    }
    if (loadingElement) {
        Sys.UI.DomElement.setVisible(ajaxContext.get_loadingElement(), true);
    }
    if (continueRequest) {
        request.add_completed(Function.createDelegate(null, function(executor) {
            Sys.Mvc.MvcHelpers._onComplete(request, ajaxOptions, ajaxContext);
        }));
        request.invoke();
    }
}
Sys.Mvc.MvcHelpers._onComplete = function Sys_Mvc_MvcHelpers$_onComplete(request, ajaxOptions, ajaxContext) {
    /// <param name="request" type="Sys.Net.WebRequest">
    /// </param>
    /// <param name="ajaxOptions" type="Sys.Mvc.AjaxOptions">
    /// </param>
    /// <param name="ajaxContext" type="Sys.Mvc.AjaxContext">
    /// </param>
    ajaxContext.set_response(request.get_executor());
    if (ajaxOptions.onComplete && ajaxOptions.onComplete(ajaxContext) === false) {
        return;
    }
    var statusCode = ajaxContext.get_response().get_statusCode();
    if ((statusCode >= 200 && statusCode < 300) || statusCode === 304 || statusCode === 1223) {
        if (statusCode !== 204 && statusCode !== 304 && statusCode !== 1223) {
            var contentType = ajaxContext.get_response().getResponseHeader('Content-Type');
            if ((contentType) && (contentType.indexOf('application/x-javascript') !== -1)) {
                eval(ajaxContext.get_data());
            }
            else {
                Sys.Mvc.MvcHelpers.updateDomElement(ajaxContext.get_updateTarget(), ajaxContext.get_insertionMode(), ajaxContext.get_data());
            }
        }
        if (ajaxOptions.onSuccess) {
            ajaxOptions.onSuccess(ajaxContext);
        }
    }
    else {
        if (ajaxOptions.onFailure) {
            ajaxOptions.onFailure(ajaxContext);
        }
    }
    if (ajaxContext.get_loadingElement()) {
        Sys.UI.DomElement.setVisible(ajaxContext.get_loadingElement(), false);
    }
}
Sys.Mvc.MvcHelpers.updateDomElement = function Sys_Mvc_MvcHelpers$updateDomElement(target, insertionMode, content) {
    /// <param name="target" type="Object" domElement="true">
    /// </param>
    /// <param name="insertionMode" type="Sys.Mvc.InsertionMode">
    /// </param>
    /// <param name="content" type="String">
    /// </param>
    if (target) {
        switch (insertionMode) {
            case Sys.Mvc.InsertionMode.replace:
                target.innerHTML = content;
                break;
            case Sys.Mvc.InsertionMode.insertBefore:
                if (content && content.length > 0) {
                    target.innerHTML = content + target.innerHTML.trimStart();
                }
                break;
            case Sys.Mvc.InsertionMode.insertAfter:
                if (content && content.length > 0) {
                    target.innerHTML = target.innerHTML.trimEnd() + content;
                }
                break;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// Sys.Mvc.AsyncForm

Sys.Mvc.AsyncForm = function Sys_Mvc_AsyncForm() {
}
Sys.Mvc.AsyncForm.handleClick = function Sys_Mvc_AsyncForm$handleClick(form, evt) {
    /// <param name="form" type="Object" domElement="true">
    /// </param>
    /// <param name="evt" type="Sys.UI.DomEvent">
    /// </param>
    var additionalInput = Sys.Mvc.MvcHelpers._serializeSubmitButton(evt.target, evt.offsetX, evt.offsetY);
    form._additionalInput = additionalInput;
}
Sys.Mvc.AsyncForm.handleSubmit = function Sys_Mvc_AsyncForm$handleSubmit(form, evt, ajaxOptions) {
    /// <param name="form" type="Object" domElement="true">
    /// </param>
    /// <param name="evt" type="Sys.UI.DomEvent">
    /// </param>
    /// <param name="ajaxOptions" type="Sys.Mvc.AjaxOptions">
    /// </param>
    evt.preventDefault();
    var validationCallbacks = form.validationCallbacks;
    if (validationCallbacks) {
        for (var i = 0; i < validationCallbacks.length; i++) {
            var callback = validationCallbacks[i];
            if (!callback()) {
                return;
            }
        }
    }
    var body = Sys.Mvc.MvcHelpers._serializeForm(form);
    Sys.Mvc.MvcHelpers._asyncRequest(form.action, form.method || 'post', body, form, ajaxOptions);
}


Sys.Mvc.AjaxContext.registerClass('Sys.Mvc.AjaxContext');
Sys.Mvc.AsyncHyperlink.registerClass('Sys.Mvc.AsyncHyperlink');
Sys.Mvc.MvcHelpers.registerClass('Sys.Mvc.MvcHelpers');
Sys.Mvc.AsyncForm.registerClass('Sys.Mvc.AsyncForm');

// ---- Do not remove this footer ----
// Generated using Script# v0.5.0.0 (http://projects.nikhilk.net)
// -----------------------------------

// SIG // Begin signature block
// SIG // MIIQTAYJKoZIhvcNAQcCoIIQPTCCEDkCAQExCzAJBgUr
// SIG // DgMCGgUAMGcGCisGAQQBgjcCAQSgWTBXMDIGCisGAQQB
// SIG // gjcCAR4wJAIBAQQQEODJBs441BGiowAQS9NQkAIBAAIB
// SIG // AAIBAAIBAAIBADAhMAkGBSsOAwIaBQAEFIMZdbb0tQdE
// SIG // x0//TitxFcBa+JC0oIIODzCCBBMwggNAoAMCAQICEGoL
// SIG // mU/AACKrEdsCQnwC074wCQYFKw4DAh0FADB1MSswKQYD
// SIG // VQQLEyJDb3B5cmlnaHQgKGMpIDE5OTkgTWljcm9zb2Z0
// SIG // IENvcnAuMR4wHAYDVQQLExVNaWNyb3NvZnQgQ29ycG9y
// SIG // YXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUZXN0IFJv
// SIG // b3QgQXV0aG9yaXR5MB4XDTA2MDYyMjIyNTczMVoXDTEx
// SIG // MDYyMTA3MDAwMFowcTELMAkGA1UEBhMCVVMxEzARBgNV
// SIG // BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
// SIG // HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEb
// SIG // MBkGA1UEAxMSTWljcm9zb2Z0IFRlc3QgUENBMIIBIjAN
// SIG // BgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAj/Pz33qn
// SIG // cihhfpDzgWdPPEKAs8NyTe9/EGW4StfGTaxnm6+j/cTt
// SIG // fDRsVXNecQkcoKI69WVT1NzP8zOjWjMsV81IIbelJDAx
// SIG // UzWp2tnbdH9MLnhnzdvJ7bGPt67/eW+sIwZUDiNDN3jd
// SIG // Pk4KbdAq9sZ+W5J0DbMTD1yxcbQQ/LEgCAgueW5f0nI0
// SIG // rpI6gbAyrM5DWTCmwfyu+MzofYZrXK7r3pX6Kjl1BlxB
// SIG // OlHcVzVOksssnXuk3Jrp/iGcYR87pEx/UrGFOWR9kYlv
// SIG // nhRCs7yi2moXhyTmG9V8fY+q3ALJoV7d/YEqnybDNkHT
// SIG // z/xzDRx0KDjypQrF0Q+7077QkwIDAQABo4HrMIHoMIGo
// SIG // BgNVHQEEgaAwgZ2AEMBjRdejAX15xXp6XyjbQ9ahdzB1
// SIG // MSswKQYDVQQLEyJDb3B5cmlnaHQgKGMpIDE5OTkgTWlj
// SIG // cm9zb2Z0IENvcnAuMR4wHAYDVQQLExVNaWNyb3NvZnQg
// SIG // Q29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBU
// SIG // ZXN0IFJvb3QgQXV0aG9yaXR5ghBf6k/S8h1DELboVD7Y
// SIG // lSYYMA8GA1UdEwEB/wQFMAMBAf8wHQYDVR0OBBYEFFSl
// SIG // IUygrm+cYE4Pzt1G1ddh1hesMAsGA1UdDwQEAwIBhjAJ
// SIG // BgUrDgMCHQUAA4HBACzODwWw7h9lGeKjJ7yc936jJard
// SIG // LMfrxQKBMZfJTb9MWDDIJ9WniM6epQ7vmTWM9Q4cLMy2
// SIG // kMGgdc3mffQLETF6g/v+aEzFG5tUqingK125JFP57MGc
// SIG // JYMlQGO3KUIcedPC8cyj+oYwi6tbSpDLRCCQ7MAFS15r
// SIG // 4Dnxn783pZ5nSXh1o+NrSz5mbGusDIj0ujHBCqblI96+
// SIG // Rk7oVQ2DI3oQkSmGQf+BrmRXoJfB3YuXXFc+F88beLHS
// SIG // F0S8oJhPjzCCBKgwggOQoAMCAQICCmEBi3MAAAAAABMw
// SIG // DQYJKoZIhvcNAQEFBQAwcTELMAkGA1UEBhMCVVMxEzAR
// SIG // BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
// SIG // bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
// SIG // bjEbMBkGA1UEAxMSTWljcm9zb2Z0IFRlc3QgUENBMB4X
// SIG // DTA5MDgxNzIzMjAxN1oXDTExMDYyMTA3MDAwMFowgYEx
// SIG // EzARBgoJkiaJk/IsZAEZFgNjb20xGTAXBgoJkiaJk/Is
// SIG // ZAEZFgltaWNyb3NvZnQxFDASBgoJkiaJk/IsZAEZFgRj
// SIG // b3JwMRcwFQYKCZImiZPyLGQBGRYHcmVkbW9uZDEgMB4G
// SIG // A1UEAxMXTVNJVCBUZXN0IENvZGVTaWduIENBIDEwggEi
// SIG // MA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDKz+fW
// SIG // ilZnvB1mb2XQEkuK0GeO6we7n8RfXKMFTp9ifiOnD0v5
// SIG // FFYrPjAKGMrOxroVu8rPTOPukz6hlYdMzIkV68iyS4FU
// SIG // ZjQGz5wQNnLKbUN1PFlP+NsWJZjzvRuZv9WWweCKnUeE
// SIG // Fxur+rzMtvz50aVAechNt36xI6rIxVXRv5xvDKzkKTGv
// SIG // BmaP0YsqkNcUe3GJy17yWoEWX+kKGX69xNezEai06On2
// SIG // cpKToU0ibyRNhgs2Ygzb5U/9hISMYt7YFdEYggL0zTNp
// SIG // 59hmfaB5FT0yMor1iUcSFVtTGObPmB1dsD4EPcYSTZtp
// SIG // 5R4hzYecLp8kSV78s1ycVDt5pQY1AgMBAAGjggEvMIIB
// SIG // KzAQBgkrBgEEAYI3FQEEAwIBADAdBgNVHQ4EFgQUhOTQ
// SIG // p5jIj+9WN5bdvfFGrMW5xZ4wGQYJKwYBBAGCNxQCBAwe
// SIG // CgBTAHUAYgBDAEEwCwYDVR0PBAQDAgGGMA8GA1UdEwEB
// SIG // /wQFMAMBAf8wHwYDVR0jBBgwFoAUVKUhTKCub5xgTg/O
// SIG // 3UbV12HWF6wwTAYDVR0fBEUwQzBBoD+gPYY7aHR0cDov
// SIG // L2NybC5taWNyb3NvZnQuY29tL3BraS9jcmwvcHJvZHVj
// SIG // dHMvbGVnYWN5dGVzdHBjYS5jcmwwUAYIKwYBBQUHAQEE
// SIG // RDBCMEAGCCsGAQUFBzAChjRodHRwOi8vd3d3Lm1pY3Jv
// SIG // c29mdC5jb20vcGtpL2NlcnRzL0xlZ2FjeVRlc3RQQ0Eu
// SIG // Y3J0MA0GCSqGSIb3DQEBBQUAA4IBAQA4GSVNJtByD1os
// SIG // xEzGCLI18ykM+RrR02D1DyopRstCY+OoOeX5WX5BVknd
// SIG // j0w6P1Ea4TD450ozSN7q1yWQcgIT2K8DbwyKTnDn5enx
// SIG // josg2n+ljxnLputPDiFAdfNP+XHew9x/gB+JR7oSK/Ps
// SIG // LzXbuVITIRDkPogIJUFQMrwKI9o0bv2sLWV+fSk+fEXB
// SIG // OaHysKBGU+EIhjrfHx4QP38jQUi2yJZQ85klqVVuSL21
// SIG // dwIP5QZiYplN6zicK6ez3r+yozQLOg6mc5MBgrUPBTsV
// SIG // sSbHM2+BGVaorOyI7JMr0sHBl6IGFbqRIPqtWY4rimD8
// SIG // uNi6hHfLJFmTMDstbNJEMIIFSDCCBDCgAwIBAgIKa4DO
// SIG // qQAAAACmmDANBgkqhkiG9w0BAQUFADCBgTETMBEGCgmS
// SIG // JomT8ixkARkWA2NvbTEZMBcGCgmSJomT8ixkARkWCW1p
// SIG // Y3Jvc29mdDEUMBIGCgmSJomT8ixkARkWBGNvcnAxFzAV
// SIG // BgoJkiaJk/IsZAEZFgdyZWRtb25kMSAwHgYDVQQDExdN
// SIG // U0lUIFRlc3QgQ29kZVNpZ24gQ0EgMTAeFw0wOTExMDYx
// SIG // ODE3MDdaFw0xMDExMDYxODE3MDdaMBUxEzARBgNVBAMT
// SIG // ClZTIEJsZCBMYWIwgZ8wDQYJKoZIhvcNAQEBBQADgY0A
// SIG // MIGJAoGBAJMiPNeJy8vp5oeABJLebUDw5LUKy+N3pOFp
// SIG // h5QGJmE4b4JgN2LEXNVLh6lOle35xLCbQOJCVs1eDOgq
// SIG // puOWq5EvFYOugrxGcS4wfHNt4/Rwjigo/UQDYU755puL
// SIG // RBqLVtGqlcMYwLhzAWV0R7HWtmBDfhqAH19O3P3foI2X
// SIG // zrLrAgMBAAGjggKvMIICqzALBgNVHQ8EBAMCB4AwHQYD
// SIG // VR0OBBYEFAjpDmzPyPih2x+qdItA5Ul2ZAe3MD0GCSsG
// SIG // AQQBgjcVBwQwMC4GJisGAQQBgjcVCIPPiU2t8gKFoZ8M
// SIG // gvrKfYHh+3SBT4KusGqH9P0yAgFkAgEMMB8GA1UdIwQY
// SIG // MBaAFITk0KeYyI/vVjeW3b3xRqzFucWeMIHoBgNVHR8E
// SIG // geAwgd0wgdqggdeggdSGNmh0dHA6Ly9jb3JwcGtpL2Ny
// SIG // bC9NU0lUJTIwVGVzdCUyMENvZGVTaWduJTIwQ0ElMjAx
// SIG // LmNybIZNaHR0cDovL21zY3JsLm1pY3Jvc29mdC5jb20v
// SIG // cGtpL21zY29ycC9jcmwvTVNJVCUyMFRlc3QlMjBDb2Rl
// SIG // U2lnbiUyMENBJTIwMS5jcmyGS2h0dHA6Ly9jcmwubWlj
// SIG // cm9zb2Z0LmNvbS9wa2kvbXNjb3JwL2NybC9NU0lUJTIw
// SIG // VGVzdCUyMENvZGVTaWduJTIwQ0ElMjAxLmNybDCBqQYI
// SIG // KwYBBQUHAQEEgZwwgZkwQgYIKwYBBQUHMAKGNmh0dHA6
// SIG // Ly9jb3JwcGtpL2FpYS9NU0lUJTIwVGVzdCUyMENvZGVT
// SIG // aWduJTIwQ0ElMjAxLmNydDBTBggrBgEFBQcwAoZHaHR0
// SIG // cDovL3d3dy5taWNyb3NvZnQuY29tL3BraS9tc2NvcnAv
// SIG // TVNJVCUyMFRlc3QlMjBDb2RlU2lnbiUyMENBJTIwMS5j
// SIG // cnQwHwYDVR0lBBgwFgYKKwYBBAGCNwoDBgYIKwYBBQUH
// SIG // AwMwKQYJKwYBBAGCNxUKBBwwGjAMBgorBgEEAYI3CgMG
// SIG // MAoGCCsGAQUFBwMDMDoGA1UdEQQzMDGgLwYKKwYBBAGC
// SIG // NxQCA6AhDB9kbGFiQHJlZG1vbmQuY29ycC5taWNyb3Nv
// SIG // ZnQuY29tMA0GCSqGSIb3DQEBBQUAA4IBAQBqcp669vuu
// SIG // QzcKv0NTjeY2jhqSYRlwon/Q83ON8GCb1vf3AEFmwPNI
// SIG // 5hxSmGpqr4JrfuJFFa6SxO8praB4oaZeTKt7bAH/uRpb
// SIG // HP3U8Y6tuJAzfWaYUiNoF02lpgFEa44pw3sGJ3XA6uj0
// SIG // cG4jo1U5b81pkFblA4WRIuU1VHUDmARJbinQVt3JAFyU
// SIG // /J4SuAMUxraGUS8voUpk/Jyy8A7dhNepQQmc8BlY6lIQ
// SIG // fyU6WYQhOSuuQO5mfZhJaFGA53gqWzJfVBD32i7O6lAt
// SIG // /SXE7oV+Fwo5FHC8dOMzIn4bITvDQxgfO0M530uBmnCY
// SIG // qsRRYNNgYql6JvUjP/DSy6ZfMYIBqTCCAaUCAQEwgZAw
// SIG // gYExEzARBgoJkiaJk/IsZAEZFgNjb20xGTAXBgoJkiaJ
// SIG // k/IsZAEZFgltaWNyb3NvZnQxFDASBgoJkiaJk/IsZAEZ
// SIG // FgRjb3JwMRcwFQYKCZImiZPyLGQBGRYHcmVkbW9uZDEg
// SIG // MB4GA1UEAxMXTVNJVCBUZXN0IENvZGVTaWduIENBIDEC
// SIG // CmuAzqkAAAAAppgwCQYFKw4DAhoFAKBwMBAGCisGAQQB
// SIG // gjcCAQwxAjAAMBkGCSqGSIb3DQEJAzEMBgorBgEEAYI3
// SIG // AgEEMBwGCisGAQQBgjcCAQsxDjAMBgorBgEEAYI3AgEV
// SIG // MCMGCSqGSIb3DQEJBDEWBBSlL/Uo/pjW8WsgZLTQ7te3
// SIG // pCkViTANBgkqhkiG9w0BAQEFAASBgGM9xUl4DjF3aFRm
// SIG // 6WqWnAd8q0VBG7ImLP7K6pW3AK8n8zJEf8E9+IAB3/Hw
// SIG // 08j13L0w7j/WFsiTbcNkTEVjNgVLc0uN0RkPkSOwW4hr
// SIG // 060HvIe7vnL6vuyQ/VpE/AKEUb1VhFlxUeDL1zoYvN92
// SIG // 5gsVZTAMVSNCVoloUBsAvcBd
// SIG // End signature block
