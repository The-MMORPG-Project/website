const { ipcRenderer } = require('electron')
const launchBarProgress = document.getElementById('launchBarProgress')

let curProgress = 0
let prevProgress = 0
let animProgress = 0

ipcRenderer.on('progress', (event, arg) => {
	curProgress = arg

	launchBarProgress.animate([
		{
		  transform: `scaleX(${0})`,
		},
		{
		  tranform: `scaleX(${100})`,
		},
	  ], {
		duration: 300,
		fill: "forwards",
	  });
})

function launch() {
	ipcRenderer.send('download-button', { url: 'https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png' })
}

function lerp(v0, v1, t) {
	return (1 - t) * v0 + t * v1;
}