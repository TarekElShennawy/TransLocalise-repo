window.onload = function () {
	//Get create project button
	const createProjectButton = document.getElementById("createProjectButton");

	//Onclick redirect user to create project view
	createProjectButton.addEventListener("click", function () {
		const url = this.dataset.projectCreateUrl;

		window.location.href = url;
	});

	//Getting delete buttons
	const deleteButtons = document.querySelectorAll(".deleteButton");

	//Onclick the projectId is passed to the modal form to delete when user confirms deletion.
	deleteButtons.forEach(button => {
		button.addEventListener("click", function () {
			const projectId = this.dataset.projectId;
			setProjectIdInModal(projectId);
		})		
	})
};

//Setting auto-hide for the bootstrap alert
window.setTimeout(function () {
	$(".alert").fadeTo(500, 0).slideUp(500, function () { //slowly fade the alert as it slides up
		$(this).remove(); //Then finally remove it completely.
	})
}, 4000);

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
	$('#deleteModal input[name="projectId"]').val(projectId);
}