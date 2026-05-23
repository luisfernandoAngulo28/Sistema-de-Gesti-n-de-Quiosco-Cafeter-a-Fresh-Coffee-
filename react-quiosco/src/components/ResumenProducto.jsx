import useQuisco from "../hooks/useQuiosco"
import { formatearDinero } from "../helpers"

export default function ResumenProducto({ producto, compacto = false }) {
    const { handleEditarCantidad, handleEliminarProductoPedido } = useQuisco()
    const { id, nombre, precio, cantidad, imagen } = producto

    return (
        <div className={`flex items-center gap-2.5 bg-gray-50 dark:bg-gray-800/60 rounded-xl group ${compacto ? 'p-2' : 'p-3'}`}>
            <div className={`${compacto ? 'w-9 h-9' : 'w-12 h-12'} rounded-xl overflow-hidden flex-shrink-0 bg-gray-200 dark:bg-gray-700`}>
                <img
                    src={`/img/${imagen}.jpg`}
                    alt={nombre}
                    className="w-full h-full object-cover"
                    onError={e => { e.target.style.display = 'none' }}
                />
            </div>

            <div className="flex-1 min-w-0">
                <p className="text-xs font-semibold text-gray-800 dark:text-gray-100 leading-tight truncate">{nombre}</p>
                <p className="text-xs font-black text-amber-500 mt-0.5">{formatearDinero(precio * cantidad)}</p>
                <span className="text-xs text-gray-400 dark:text-gray-500">x{cantidad} × {formatearDinero(precio)}</span>
            </div>

            <div className="flex flex-col gap-1 opacity-0 group-hover:opacity-100 transition-opacity duration-150 flex-shrink-0">
                <button
                    type="button"
                    onClick={() => handleEditarCantidad(id)}
                    className="w-6 h-6 flex items-center justify-center rounded-lg bg-indigo-50 dark:bg-indigo-900/30 text-indigo-500 hover:bg-indigo-100 dark:hover:bg-indigo-900/50 transition-colors"
                    title="Editar"
                >
                    <svg xmlns="http://www.w3.org/2000/svg" className="h-3 w-3" viewBox="0 0 20 20" fill="currentColor">
                        <path d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z" />
                    </svg>
                </button>
                <button
                    type="button"
                    onClick={() => handleEliminarProductoPedido(id)}
                    className="w-6 h-6 flex items-center justify-center rounded-lg bg-red-50 dark:bg-red-900/20 text-red-400 hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors"
                    title="Eliminar"
                >
                    <svg xmlns="http://www.w3.org/2000/svg" className="h-3 w-3" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                        <path strokeLinecap="round" strokeLinejoin="round" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                    </svg>
                </button>
            </div>
        </div>
    )
}
