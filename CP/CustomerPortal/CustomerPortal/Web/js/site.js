(function() {

	function bustFrame() {
		if (top != self) {
			top.location.replace(self.location.href);
		}
	}

	window.onload = bustFrame;

})();

var asirraChallengePassed = false;
var asirraProtectedButtonId;

function validateAsirraChallenge(buttonId) {

	if (asirraChallengePassed) {
		return true;
	}

	asirraProtectedButtonId = buttonId;
	
	Asirra_CheckIfHuman(humanCheckComplete);

	return false;
}

function humanCheckComplete(isHuman) {
	if (!isHuman) {
		alert('The identified cats do not appear to be correct. Please try again.');
	}
	else {
		asirraChallengePassed = true;
		document.getElementById(asirraProtectedButtonId).click();
	}
}
