import { app, BrowserWindow, Menu, ipcMain } from 'electron'
import { download } from 'electron-dl'
import AdmZip from 'adm-zip'
import isDev from 'electron-is-dev'
import fs from 'fs'
import { promisify } from 'util'
import { execFile as child } from 'child_process'

const unlinkAsync = promisify(fs.unlink)

let mainWin: BrowserWindow
let downloading: boolean
let file: string

const createMainWindow = () => {
	mainWin = new BrowserWindow({
		width: 600,
		height: 400,
		title: 'Launcher',
		show: false,
		center: true,
		resizable: true,
		frame: false,
		webPreferences: {
			nodeIntegration: true,
			enableRemoteModule: true
		}
	})

	mainWin.on('ready-to-show', () => {
		mainWin.show()
	})

	mainWin.loadFile('./src/html/index.html')
}

app.on('ready', () => {
	clearConsole()

	app.setAppUserModelId(process.execPath)

	createMainWindow()
	Menu.setApplicationMenu(null)
	initDevTools()

	mainWin.on('closed', () => {
		app.quit()
	})

	console.log('Electron app is up and running..')
})

ipcMain.on('download-button', async (event, { url }) => {
	console.log('donwloading...')
	downloading = true

	// Extract file name from url
	const split = url.split('/')
	file = split[split.length - 1]

	const win = BrowserWindow.getFocusedWindow()
	const options = {
		directory: app.getPath('pictures'),
		onProgress
	}
	await download(win as any, url, options)
})

const onProgress = async (obj: { percent: number }) => {
	mainWin.webContents.send('progress', obj.percent)

	if (obj.percent == 1 && downloading) {
		downloading = false

		// We delay this part as the electron-dl onProgress event outputs multiple '1's when finished
		// Perhaps later on this could be replaced with a function that checks if the zip exists before extracting it
		setTimeout(async function () {
			const releaseDir = app.getPath('pictures')
			const sourceZip = `${releaseDir}/${file}`

			// Extract
			console.log('extracting...')
			const zip = new AdmZip(sourceZip)
			zip.extractAllTo(`${releaseDir}`)

			// Delete latest.zip since we no longer need it
			await unlinkAsync(sourceZip)

			// Run the program
			// -screen-fullscreen (must be 1 or 0)
			// -screen-width (must be a supported resolution)
			// -screen-height (must be a supported resolution)
			// -screen-quality (must be a quality setting name)
			// https://docs.unity3d.com/Manual/CommandLineArguments.html
			const parameters = ['-screen-fullscreen 1', '-screen-width 1920', '-screen-height 1080'];
			child(`${releaseDir}/${file.split('.')[0]}/Networking.exe`, parameters, (err, data) => {
				console.log(err)
				console.log(data.toString())
			})
		}, 1000)
	}
}

const initDevTools = () => {
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
		const menu = Menu.buildFromTemplate(devMenuTemplate as any)
		Menu.setApplicationMenu(menu)
	}
}

const clearConsole = () => {
	const readline = require('readline')
	const blank = '\n'.repeat(process.stdout.rows)
	console.log(blank)
	readline.cursorTo(process.stdout, 0, 0)
	readline.clearScreenDown(process.stdout)
}
