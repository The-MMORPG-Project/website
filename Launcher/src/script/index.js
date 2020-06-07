const { ipcRenderer } = require('electron')
const { BrowserWindow } = require('electron').remote

const elements = {
	launchBarProgress: id('launchBarProgress'),
	launchButton: id('launchButton'),
	settingsButton: id('settingsButton'),
	close: id('close'),
	minimize: id('minimize'),
	account: id('account')
}

let settingsWin = null
let authWin = null

let renderInterval

let downloading = false
let settingsOpen = false
let authOpen = false

let progress = 0
let width = 0

let webIP = 'localhost'
let webPort = 3000

// We received a new download progress value
ipcRenderer.on('progress', (event, arg) => {
	progress = Math.round(arg * 100)
})

// Render the download progress bar
function renderProgressBar() {
	if (width >= 100) {
		clearInterval(renderInterval)
		downloading = false

		new Notification('Update Complete', {
			body: 'Client is now up-to-date.'
		})
		return
	}

	if (progress > width) {
		width = round(lerp(width, progress, progress / 100), 2)
		elements.launchBarProgress.style.width = width + '%'
	}

	elements.launchButton.textContent = progress + '%'
}

// Menu Buttons
elements.close.addEventListener('click', () => {
	BrowserWindow.getFocusedWindow().close()
})

elements.minimize.addEventListener('click', () => {
	BrowserWindow.getFocusedWindow().minimize()
})

elements.settingsButton.addEventListener('click', () => {
	if (settingsOpen) {
		return
	}

	settingsOpen = true

	settingsWin = new BrowserWindow({
		width: 400,
		height: 300,
		title: 'Settings',
		show: false,
		frame: false,
		parent: BrowserWindow.fromId(1),
		modal: true,
		webPreferences: {
			nodeIntegration: true,
			enableRemoteModule: true
		}
	})

	settingsWin.on('ready-to-show', () => {
		settingsWin.moveTop()
		settingsWin.show()
	})

	settingsWin.on('closed', () => {
		settingsWin = null
		settingsOpen = false
	})

	settingsWin.loadFile('./src/html/settings.html')
})

elements.account.addEventListener('click', () => {
	if (authOpen) {
		return
	}

	authOpen = true

	authWin = new BrowserWindow({
		width: 600,
		height: 300,
		title: 'Authentication',
		show: false,
		frame: false,
		resizable: false,
		parent: BrowserWindow.fromId(1),
		modal: true,
		webPreferences: {
			nodeIntegration: true,
			enableRemoteModule: true
		}
	})

	authWin.on('ready-to-show', () => {
		authWin.moveTop()
		authWin.show()
	})

	authWin.on('closed', () => {
		authWin = null
		authOpen = false
	})

	authWin.loadFile('./src/html/auth.html')
})

// Launch Button
elements.launchButton.addEventListener('click', () => {
	if (downloading) { // Only launch if not downloading anything
		return
	}

	const version = 'latest'
	const platform = process.platform
	console.log(platform)

	// Tell the main process what to download
	ipcRenderer.send('download-button', { url: `http://${webIP}:${webPort}/api/releases/${platform}/${version}.zip` })
	
	// Reset values
	width = 0
	progress = 0
	elements.launchBarProgress.style.width = '0'

	// Indicate that we are now downloading
	downloading = true

	// Start rendering the download progress bar every 10ms
	renderInterval = setInterval(renderProgressBar, 10)
})