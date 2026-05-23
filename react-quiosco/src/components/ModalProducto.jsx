import { useState, useEffect } from 'react'
import useQuisco from "../hooks/useQuiosco"
import { formatearDinero } from "../helpers"

export default function ModalProducto() {
    const { producto, handleClickModal, handleAgregarPedido, pedido } = useQuisco()
    const [cantidad, setCantidad] = useState(1)
    const [edicion, setEdicion] = useState(false)

    useEffect(() => {
        if (pedido.some(p => p.id === producto.id)) {
            const productoEdicion = pedido.find(p => p.id === producto.id)
            setCantidad(productoEdicion.cantidad)
            setEdicion(true)
        }
    }, [pedido])

    return (
        <div className="md:flex items-stretch gap-0 w-full max-w-2xl bg-white dark:bg-gray-900">
            <div className="md:w-2/5 rounded-l-2xl overflow-hidden">
                <img
                    alt={producto.nombre}
                    src={`/img/${producto.imagen}.jpg`}
                    className="w-full h-full object-cover min-h-48"
                />
            </div>

            <div className="md:w-3/5 p-6 flex flex-col">
                <div className="flex justify-between items-start mb-4">
                    <div>
                        <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">{producto.nombre}</h2>
                        <p className="text-3xl font-black text-amber-500 mt-1">
                            {formatearDinero(producto.precio)}
                        </p>
                    </div>
                    <button
                        onClick={handleClickModal}
                        className="w-8 h-8 flex items-center justify-center rounded-full bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600 text-gray-500 dark:text-gray-400 transition-colors flex-shrink-0"
                    >
                        <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                            <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12" />
                        </svg>
                    </button>
                </div>

                <p className="text-sm text-gray-400 dark:text-gray-500 mb-6">Selecciona la cantidad deseada</p>

                <div className="flex items-center gap-4 mb-6">
                    <button
                        type="button"
                        onClick={() => { if (cantidad > 1) setCantidad(cantidad - 1) }}
                        disabled={cantidad <= 1}
                        className="w-10 h-10 rounded-full border-2 border-gray-200 dark:border-gray-600 flex items-center justify-center text-gray-600 dark:text-gray-400 hover:border-indigo-400 hover:text-indigo-600 disabled:opacity-30 transition-colors"
                    >
                        <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                            <path strokeLinecap="round" strokeLinejoin="round" d="M20 12H4" />
                        </svg>
                    </button>

                    <span className="text-4xl font-black text-gray-800 dark:text-gray-100 w-8 text-center">{cantidad}</span>

                    <button
                        type="button"
                        onClick={() => { if (cantidad < 5) setCantidad(cantidad + 1) }}
                        disabled={cantidad >= 5}
                        className="w-10 h-10 rounded-full border-2 border-gray-200 dark:border-gray-600 flex items-center justify-center text-gray-600 dark:text-gray-400 hover:border-indigo-400 hover:text-indigo-600 disabled:opacity-30 transition-colors"
                    >
                        <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                            <path strokeLinecap="round" strokeLinejoin="round" d="M12 4v16m8-8H4" />
                        </svg>
                    </button>
                </div>

                <div className="mt-auto border-t border-gray-100 dark:border-gray-700/50 pt-4">
                    <div className="flex justify-between items-center mb-4">
                        <span className="text-sm text-gray-500 dark:text-gray-400 font-medium">Subtotal</span>
                        <span className="text-xl font-black text-gray-800 dark:text-gray-100">
                            {formatearDinero(producto.precio * cantidad)}
                        </span>
                    </div>

                    <button
                        type="button"
                        className="w-full bg-indigo-600 hover:bg-indigo-700 text-white py-3 rounded-xl font-bold text-sm uppercase transition-colors duration-150"
                        onClick={() => {
                            handleAgregarPedido({ ...producto, cantidad })
                            handleClickModal()
                        }}
                    >
                        {edicion ? 'Guardar Cambios' : 'Añadir al Pedido'}
                    </button>
                </div>
            </div>
        </div>
    )
}
