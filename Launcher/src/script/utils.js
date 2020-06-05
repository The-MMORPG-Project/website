const id = (id) => document.getElementById(id)
const round = (n, d) => Math.round(n * (10 ** d)) / (10 ** d)
const lerp = (v0, v1, t) => (1 - t) * v0 + t * v1