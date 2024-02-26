window.onload = function () {
	var userSelect = document.getElementById("selectedUser");

	///check if user selections exist
	if (userSelect.options.length > 0) {
		//If so, set the selected Id as the first value until changed
		updateSelectedUserId(userSelect.options[0].value)
	}
};

//Getting the "View projects" buttons
const toggleButtons = document.querySelectorAll('.toggle-button');

//"View buttons" Buttons listening for a click to view the project details
toggleButtons.forEach(button => {
	const targetCollapse = document.querySelector(button.dataset.bsTarget);
	button.addEventListener('click', () => {
		targetCollapse.classList.toggle('show');
	});
});

function setProjectIdInModal(projectId) {
	$('#myModal input[name="projectId"]').val(projectId);
}

//Updating the selectedUserId input based on dropdown selection
function updateSelectedUserId(userId) {
	document.getElementById("selectedUserId").value = userId;
}