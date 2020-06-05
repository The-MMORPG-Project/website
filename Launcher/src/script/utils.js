function id(id) {
    return document.getElementById(id)
}

function round(n, d) {
    return Math.round(n * (10 ** d)) / (10 ** d)
}

function lerp(v0, v1, t) {
	return (1 - t) * v0 + t * v1
}