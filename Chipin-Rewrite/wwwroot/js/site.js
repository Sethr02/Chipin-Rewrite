

// Function to toggle the sidebar

    var acc = document.getElementsByClassName("myclass");
    var k;
    for (k = 0; k < acc.length; k++) {
        acc[k].addEventListener("click", function () {
            this.classList.toggle("activex");
            var panel = this.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
        });
    }

function openSideBarFunc() {
  
    const closeButton = document.getElementById("sidebarBtnClose");
    const elementToMove = document.getElementById("sidebar");
    elementToMove.style.right = "0px";
    closeButton.style.transform = "rotate(0deg)"
    document.getElementById("overlay").style.display = "block";
}

function closeSideBarFunc() {

    const closeButton = document.getElementById("sidebarBtnClose");
    const elementToMove = document.getElementById("sidebar");
    elementToMove.style.right = "-550px";
    closeButton.style.transform = "rotate(180deg)"
    document.getElementById("overlay").style.display = "none";

}


//Expand imag
function expandImgFunc(imgs) {
    // Get the expanded image
    var expandImg = document.getElementById("expandedImg");
    // Get the image text
    var imgText = document.getElementById("imgtext");
    // Use the same src in the expanded image as the image being clicked on from the grid
    expandImg.src = imgs.src;
    // Use the value of the alt attribute of the clickable image as text inside the expanded image
    imgText.innerHTML = imgs.alt;
    // Show the container element (hidden with CSS)
    expandImg.parentElement.style.display = "block";
}



function hideImage(img) {
    // If the image failed to load, remove it from the DOM

    const liElement = img.closest("li");
    if (liElement) {
        liElement.remove();
    }

}



/* Toggle between adding and removing the "responsive" class to topnav when the user clicks on the icon */
function myFunction() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}



function toggleNav() {
    const navLinks = document.querySelector('.nav-links');
    navLinks.style.display = navLinks.style.display === 'flex' ? 'none' : 'flex';
}

function refreshNavOnResize() {
    const navLinks = document.querySelector('.nav-links');
    const expandIcon = document.querySelector('.expand-icon');
    const logo = document.querySelector('.logo');

    // Show navigation links and hide the expand icon on larger screens
    function showNavOnResize() {
        const elementToMove = document.getElementById("sidebar");
        if (window.innerWidth > 500) {

            navLinks.style.display = 'flex';
            expandIcon.style.display = 'none';
            elementToMove.style.right = "-550px";
            document.getElementById("overlay").style.display = "none";
            logo.style.marginLeft = '5px';
        } else {
            navLinks.style.display = 'none';
            expandIcon.style.display = 'block';
            logo.style.margin = 'auto';

        }
    }

    // Call the function initially to set the navigation state on page load
    showNavOnResize();

    // Add event listener to trigger the function on window resize
    window.addEventListener('resize', showNavOnResize);
}

// Call the refreshNavOnResize function when the DOM is ready
document.addEventListener('DOMContentLoaded', refreshNavOnResize);