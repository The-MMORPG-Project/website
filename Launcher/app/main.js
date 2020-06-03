const { app, BrowserWindow, Menu, ipcMain } = require('electron')
const { download } = require('electron-dl')
const isDev = require('electron-is-dev')

let mainWin = null
let settingsWin = null

function createMainWindow() {
	mainWin = new BrowserWindow({
		width: 600,
		height: 400,
		center: true,
		resizable: true,
		webPreferences: {
			nodeIntegration: true
		}
	})

	mainWin.loadFile('../src/index.html')
}

function createSettingsWindow() {
	settingsWin = new BrowserWindow({
		width: 200,
		height: 300,
		title: 'Settings'
	})

	settingsWin.loadFile('../src/settings.html')

	settingsWin.on('close', () => {
		settingsWin = null
	})
}

app.on('ready', () => {
	clearConsole()
	createMainWindow()
	createSettingsWindow()
	Menu.setApplicationMenu(null)
	initDevTools()

	mainWin.on('closed', () => {
		app.quit()
	})

	console.log('Electron app is up and running..')
})

ipcMain.on('download-button', async (event, { url }) => {
	const win = BrowserWindow.getFocusedWindow()
	const options = {
		directory: app.getPath('pictures'),
		onProgress
	}
	await download(win, url, options)
})

function onProgress(obj) {
	mainWin.webContents.send('progress', obj.percent)
}

function initDevTools() {
	if (isDev) {
		const devMenuTemplate = [
			{
				label: 'Developer Tools',
				submenu: [
					{
						label: 'Toggle DevTools',
						accelerator: 'CmdOrCtrl+Shift+I',
						click(item, focusedWindow) {
							focusedWindow.toggleDevTools()
						}
					},
					{
						role: 'reload'
					}
				]
			}
		]
		const menu = Menu.buildFromTemplate(devMenuTemplate)
		Menu.setApplicationMenu(menu)
	}
}

function clearConsole() {
	const readline = require('readline')
	const blank = '\n'.repeat(process.stdout.rows)
	console.log(blank)
	readline.cursorTo(process.stdout, 0, 0)
	readline.clearScreenDown(process.stdout)
}