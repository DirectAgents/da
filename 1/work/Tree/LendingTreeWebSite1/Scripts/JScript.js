function CheckValue(component, text) {
    if (component.value == text) {
        component.value = "";
    }
    else if (component.value == "") {
        component.value = text;
    }
}
var zipRequiredMessage = "enter property ZIPCODE"; // TODO: dup
var firstNameRequiredMessage = "enter first name";
