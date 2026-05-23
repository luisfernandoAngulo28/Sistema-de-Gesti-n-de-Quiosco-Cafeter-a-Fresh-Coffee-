import { formatearDinero } from "../helpers"
import useQuisco from "../hooks/useQuiosco"

export default function Producto({ producto, botonAgregar = false, botonDisponible = false }) {
    const { handleClickModal, handleSetProducto, handleClickProductoAgotado, pedido, productoRecienAgregado } = useQuisco()
    const { nombre, imagen, precio, disponible } = producto

    const enPedido   = pedido.find(p => p.id === producto.id)
    const flashActivo = productoRecienAgregado === producto.id

    return (
        <div className={`group relative bg-white dark:bg-gray-800/80 rounded-2xl overflow-hidden transition-all duration-300 flex flex-col
            shadow-sm hover:shadow-lg dark:shadow-none dark:hover:shadow-indigo-900/20
            ${flashActivo
                ? 'ring-2 ring-green-400 dark:ring-green-500 scale-[1.02]'
                : 'ring-1 ring-gray-100 dark:ring-gray-700/60 hover:-translate-y-1'
            }`}
        >
            {/* Imagen */}
            <div className="relative h-48 overflow-hidden bg-gray-100 dark:bg-gray-700/50">
                <img
                    alt={nombre}
                    className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                    src={`/img/${imagen}.jpg`}
                />
                {/* Overlay en hover */}
                <div className="absolute inset-0 bg-gradient-to-t from-black/50 via-black/10 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

                {/* Badge cantidad en carrito */}
                {enPedido && (
                    <div className={`absolute top-3 right-3 w-7 h-7 rounded-full flex items-center justify-center text-white text-xs font-black shadow-lg transition-all duration-200
                        ${flashActivo ? 'bg-green-500 scale-125' : 'bg-indigo-600'}`}
                    >
                        {enPedido.cantidad}
                    </div>
                )}

                {/* Badge agotado */}
                {botonDisponible && !disponible && (
                    <span className="absolute top-3 left-3 bg-red-500 text-white text-xs font-bold px-2.5 py-1 rounded-full shadow">
                        AGOTADO
                    </span>
                )}

                {/* Botón agregar flotante en hover */}
                {botonAgregar && (
                    <div className="absolute bottom-0 left-0 right-0 p-3 translate-y-full group-hover:translate-y-0 transition-transform duration-300">
                        <button
                            type="button"
                            onClick={() => { handleClickModal(); handleSetProducto(producto) }}
                            className="w-full bg-white/95 dark:bg-gray-900/95 backdrop-blur-sm text-indigo-600 dark:text-indigo-400 font-bold text-xs py-2 rounded-xl shadow-md hover:bg-indigo-600 hover:text-white dark:hover:bg-indigo-600 dark:hover:text-white transition-colors duration-150"
                        >
                            {enPedido ? '✎ Editar cantidad' : '+ Agregar al pedido'}
                        </button>
                    </div>
                )}
            </div>

            {/* Contenido */}
            <div className="p-4 flex flex-col flex-1">
                <h3 className="font-bold text-gray-800 dark:text-gray-100 text-sm leading-snug line-clamp-2">{nombre}</h3>

                <div className="flex items-center justify-between mt-auto pt-3 border-t border-gray-100 dark:border-gray-700/50">
                    <span className="font-black text-xl text-amber-500 dark:text-amber-400">{formatearDinero(precio)}</span>

                    {botonAgregar && (
                        <button
                            type="button"
                            className={`text-xs font-bold px-3.5 py-2 rounded-xl transition-all duration-150 active:scale-95
                                ${enPedido
                                    ? 'bg-green-50 dark:bg-green-900/30 text-green-700 dark:text-green-400 ring-1 ring-green-200 dark:ring-green-800'
                                    : 'bg-indigo-600 hover:bg-indigo-700 text-white shadow-sm shadow-indigo-200 dark:shadow-none'
                                }`}
                            onClick={() => { handleClickModal(); handleSetProducto(producto) }}
                        >
                            {enPedido ? `×${enPedido.cantidad}` : 'Agregar'}
                        </button>
                    )}

                    {botonDisponible && (
                        <button
                            type="button"
                            className={`text-xs font-bold px-3 py-2 rounded-xl transition-colors duration-150 ${
                                disponible
                                    ? 'bg-red-50 dark:bg-red-900/20 hover:bg-red-100 dark:hover:bg-red-900/40 text-red-500 dark:text-red-400'
                                    : 'bg-green-50 dark:bg-green-900/20 hover:bg-green-100 dark:hover:bg-green-900/40 text-green-600 dark:text-green-400'
                            }`}
                            onClick={() => handleClickProductoAgotado(producto.id)}
                        >
                            {disponible ? 'Agotado' : 'Activar'}
                        </button>
                    )}
                </div>
            </div>
        </div>
    )
}
