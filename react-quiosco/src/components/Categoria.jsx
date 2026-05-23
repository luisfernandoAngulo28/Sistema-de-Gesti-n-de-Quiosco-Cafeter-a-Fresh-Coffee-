import { useNavigate, useLocation } from "react-router-dom"
import useQuisco from "../hooks/useQuiosco"

export default function Categoria({ categoria }) {
    const { handleClickCategoria, categoriaActual } = useQuisco()
    const { icono, id, nombre } = categoria
    const activa = categoriaActual.id === id
    const navigate = useNavigate()
    const { pathname } = useLocation()

    const handleClick = () => {
        handleClickCategoria(id)
        if (pathname !== '/') navigate('/')
    }

    return (
        <button
            type="button"
            onClick={handleClick}
            className={`group flex items-center gap-3 w-full px-3 py-2.5 rounded-xl transition-all duration-150 cursor-pointer text-left
                ${activa
                    ? 'bg-indigo-600 text-white shadow-sm shadow-indigo-200'
                    : 'text-gray-500 dark:text-gray-400 hover:bg-indigo-50 dark:hover:bg-indigo-900/20 hover:text-indigo-600 dark:hover:text-indigo-400'
                }`}
        >
            <div className={`w-9 h-9 rounded-lg flex items-center justify-center flex-shrink-0 transition-colors duration-150
                ${activa ? 'bg-indigo-500' : 'bg-gray-100 dark:bg-gray-700/80 group-hover:bg-indigo-100 dark:group-hover:bg-indigo-900/30'}`}
            >
                <img
                    alt={nombre}
                    src={`/img/icono_${icono}.svg`}
                    className={`w-5 h-5 ${activa ? 'brightness-0 invert' : 'dark:invert dark:opacity-70'}`}
                />
            </div>
            <span className="font-semibold text-sm truncate">{nombre}</span>
            {activa && (
                <span className="ml-auto w-1.5 h-1.5 rounded-full bg-white opacity-80 flex-shrink-0" />
            )}
        </button>
    )
}
