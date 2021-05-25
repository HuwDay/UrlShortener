
function UrlSubmitted(e, elemInput) {
    if (e.keyCode == 13) {
        // Perform validation here
        if (elemInput.value == "") {
            alert("Please enter a URL");
        }
        else if (elemInput.value.indexOf('.') < 0) {
            alert("Please enter a valid URL");
        }
        else if (elemInput.value.indexOf('.') >= elemInput.value.length - 1) {
            alert("Please enter a valid URL");
        }
        else if (elemInput.value.indexOf(':') >= 0
            && elemInput.value.substring(0, elemInput.value.indexOf(':') + 1) != 'https:'
            && elemInput.value.substring(0, elemInput.value.indexOf(':') + 1) != 'http:'
        ) {
            alert("A valid URL must use either the \"http:\" or \"https:\" protocol, or omit the protocol entirely");
        }
        else {
            // If all the above validation checks pass, fire the postback event with the input element's name.
            __doPostBack(elemInput.name, '');
        }
    }

}
