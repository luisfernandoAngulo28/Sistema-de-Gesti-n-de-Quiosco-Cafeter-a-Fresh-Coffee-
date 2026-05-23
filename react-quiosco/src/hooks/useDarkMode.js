import { useState, useEffect } from 'react'

export function useDarkMode() {
    const [dark, setDark] = useState(() => {
        const saved = localStorage.getItem('DARK_MODE')
        if (saved !== null) return saved === 'true'
        return window.matchMedia('(prefers-color-scheme: dark)').matches
    })

    useEffect(() => {
        const root = document.documentElement
        root.classList.toggle('dark', dark)
        localStorage.setItem('DARK_MODE', dark)
    }, [dark])

    const toggle = () => setDark(d => !d)

    return [dark, toggle]
}
