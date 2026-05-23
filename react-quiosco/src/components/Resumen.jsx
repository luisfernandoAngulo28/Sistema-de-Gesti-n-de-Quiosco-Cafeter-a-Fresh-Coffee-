import { useNavigate } from "react-router-dom"
import useSWR from "swr"
import { Banknote, CreditCard, Smartphone, Users } from "lucide-react"
import { formatearDinero } from "../helpers"
import clienteAxios from "../config/axios"
import useQuisco from "../hooks/useQuiosco"
import ResumenProducto from "./ResumenProducto"

export default function Resumen() {
    const { pedido, total, handleSubmitNuevaOrden, numerMesa, setNumerMesa, carritoAnimado, metodoPago, setMetodoPago } = useQuisco()
    const navigate = useNavigate()

    const token = localStorage.getItem('AUTH_TOKEN')
    const { data: mesasData } = useSWR('/api/mesas', () =>
        clienteAxios('/api/mesas', { headers: { Authorization: `Bearer ${token}` } })
            .then(r => r.data.data ?? r.data)
    )
    const mesas = mesasData ?? []

    const handleSubmit = e => {
        e.preventDefault()
        handleSubmitNuevaOrden(navigate)
    }

    const vacio    = pedido.length === 0
    const compacto = pedido.length > 3

    const metodos = [
        { id: 'efectivo',      label: 'Efectivo',      Icono: Banknote },
        { id: 'tarjeta',       label: 'Tarjeta',       Icono: CreditCard },
        { id: 'transferencia', label: 'Transferencia', Icono: Smartphone },
    ]

    return (
        <aside className="w-72 h-screen flex flex-col border-l border-gray-100 dark:border-gray-700/50 bg-white dark:bg-gray-900 flex-shrink-0">
            {/* Header con ícono animado */}
            <div className="px-5 py-4 border-b border-gray-100 dark:border-gray-700/50 flex items-center gap-3">
                <div className="relative">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className={`w-6 h-6 text-gray-700 dark:text-gray-300 transition-transform duration-300 ${carritoAnimado ? 'scale-125' : 'scale-100'}`}
                        fill="none" viewBox="0 0 24 24" stroke="currentColor"
                    >
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.8} d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                    </svg>
                    {!vacio && (
                        <span className={`absolute -top-1.5 -right-1.5 w-4 h-4 bg-indigo-600 text-white text-xs font-black rounded-full flex items-center justify-center leading-none transition-transform duration-300 ${carritoAnimado ? 'scale-125' : 'scale-100'}`}>
                            {pedido.length}
                        </span>
                    )}
                </div>
                <div>
                    <h2 className="text-base font-bold text-gray-800 dark:text-gray-100 leading-tight">Mi Pedido</h2>
                    <p className="text-xs text-gray-400 dark:text-gray-500">
                        {vacio
                            ? 'Agrega productos al pedido'
                            : `${pedido.length} ${pedido.length === 1 ? 'producto seleccionado' : 'productos seleccionados'}`}
                    </p>
                </div>
            </div>

            {/* Selector de mesa */}
            <div className="px-4 py-3 border-b border-gray-50 dark:border-gray-700/50 bg-gray-50/50 dark:bg-gray-800/60">
                <label className="text-xs font-bold text-gray-500 dark:text-gray-400 uppercase tracking-wider block mb-1.5">
                    Mesa
                </label>
                <div className="flex gap-2 flex-wrap">
                    {mesas.map(mesa => {
                        const seleccionada = numerMesa === mesa.numero
                        const ocupada = mesa.ocupada && !seleccionada
                        return (
                            <button
                                key={mesa.id}
                                type="button"
                                disabled={ocupada}
                                onClick={() => setNumerMesa(seleccionada ? '' : mesa.numero)}
                                title={ocupada ? `Mesa ${mesa.numero} ocupada` : `Mesa ${mesa.numero} — ${mesa.capacidad} personas`}
                                className={`relative w-9 h-9 rounded-lg text-sm font-bold transition-all duration-150 border
                                    ${seleccionada
                                        ? 'bg-indigo-600 text-white border-indigo-600 shadow-sm'
                                        : ocupada
                                            ? 'bg-red-50 dark:bg-red-900/20 text-red-300 dark:text-red-400 border-red-100 dark:border-red-800/50 cursor-not-allowed'
                                            : 'bg-white dark:bg-gray-700 text-gray-600 dark:text-gray-300 border-gray-200 dark:border-gray-600 hover:border-indigo-300 hover:text-indigo-600'
                                    }`}
                            >
                                {mesa.numero}
                                {ocupada && (
                                    <span className="absolute -top-1 -right-1 w-2.5 h-2.5 bg-red-400 rounded-full border border-white dark:border-gray-900" />
                                )}
                            </button>
                        )
                    })}
                    {numerMesa && (
                        <button
                            type="button"
                            onClick={() => setNumerMesa('')}
                            className="w-9 h-9 rounded-lg text-xs font-bold bg-red-50 dark:bg-red-900/20 text-red-400 border border-red-100 dark:border-red-800/50 hover:bg-red-100 dark:hover:bg-red-900/40 transition-colors"
                            title="Quitar mesa"
                        >
                            ✕
                        </button>
                    )}
                </div>
                <div className="flex items-center gap-3 mt-1.5">
                    {numerMesa && (
                        <p className="text-xs text-indigo-600 font-medium">Mesa {numerMesa} seleccionada</p>
                    )}
                    {mesas.some(m => m.ocupada) && (
                        <p className="text-xs text-red-400 flex items-center gap-1">
                            <span className="w-2 h-2 bg-red-400 rounded-full inline-block" />
                            Ocupada
                        </p>
                    )}
                </div>
            </div>

            {/* Lista de productos */}
            <div className="flex-1 overflow-y-auto p-3 space-y-2">
                {vacio ? (
                    <div className="flex flex-col items-center justify-center h-full text-center py-10">
                        <div className="w-16 h-16 rounded-2xl bg-gray-50 dark:bg-gray-800 flex items-center justify-center mb-3">
                            <svg xmlns="http://www.w3.org/2000/svg" className="w-8 h-8 text-gray-200 dark:text-gray-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z" />
                            </svg>
                        </div>
                        <p className="text-sm font-semibold text-gray-300 dark:text-gray-600">Tu carrito está vacío</p>
                        <p className="text-xs text-gray-200 dark:text-gray-600 mt-1">Elige un producto del menú</p>
                    </div>
                ) : (
                    pedido.map(producto => (
                        <ResumenProducto key={producto.id} producto={producto} compacto={compacto} />
                    ))
                )}
            </div>

            {/* Método de pago */}
            {!vacio && (
                <div className="px-4 py-3 border-t border-gray-50 dark:border-gray-700/50 bg-gray-50/50 dark:bg-gray-800/60">
                    <label className="text-xs font-bold text-gray-500 dark:text-gray-400 uppercase tracking-wider block mb-1.5">
                        Método de pago
                    </label>
                    <div className="flex gap-2">
                        {metodos.map(({ id, label, Icono }) => (
                            <button
                                key={id}
                                type="button"
                                onClick={() => setMetodoPago(id)}
                                className={`flex-1 py-2 rounded-lg text-xs font-bold transition-all duration-150 border flex flex-col items-center gap-1
                                    ${metodoPago === id
                                        ? 'bg-indigo-600 text-white border-indigo-600 shadow-sm'
                                        : 'bg-white dark:bg-gray-700 text-gray-600 dark:text-gray-300 border-gray-200 dark:border-gray-600 hover:border-indigo-300 hover:text-indigo-600'
                                    }`}
                            >
                                <Icono className="w-4 h-4" />
                                <span>{label}</span>
                            </button>
                        ))}
                    </div>
                    {!metodoPago && (
                        <p className="text-xs text-amber-500 font-medium mt-1.5">Selecciona un método de pago</p>
                    )}
                </div>
            )}

            {/* Footer total + confirmar */}
            <div className="border-t border-gray-100 dark:border-gray-700/50 p-4">
                {!vacio && (
                    <div className="bg-gray-50 dark:bg-gray-800 rounded-xl p-3 mb-3">
                        <div className="flex items-center justify-between">
                            <span className="text-xs text-gray-500 dark:text-gray-400">Artículos</span>
                            <span className="text-xs text-gray-500 dark:text-gray-400">{pedido.reduce((a, p) => a + p.cantidad, 0)}</span>
                        </div>
                        <div className="flex items-center justify-between mt-1">
                            <span className="text-sm font-bold text-gray-700 dark:text-gray-200">Total</span>
                            <span className="text-2xl font-black text-gray-800 dark:text-gray-100">{formatearDinero(total)}</span>
                        </div>
                    </div>
                )}

                {vacio && (
                    <div className="flex items-center justify-between mb-3">
                        <span className="text-sm text-gray-300 dark:text-gray-600">Total</span>
                        <span className="text-xl font-black text-gray-200 dark:text-gray-600">$0.00</span>
                    </div>
                )}

                <form onSubmit={handleSubmit}>
                    <button
                        type="submit"
                        disabled={vacio || !metodoPago}
                        className={`w-full py-3.5 rounded-xl font-bold text-sm uppercase tracking-wider transition-all duration-200
                            ${vacio || !metodoPago
                                ? 'bg-gray-100 dark:bg-gray-800 text-gray-300 dark:text-gray-600 cursor-not-allowed'
                                : 'bg-indigo-600 hover:bg-indigo-700 active:scale-95 text-white shadow-md shadow-indigo-200'
                            }`}
                    >
                        {vacio ? 'Confirmar Pedido' : `Confirmar · ${formatearDinero(total)}`}
                    </button>
                </form>
            </div>
        </aside>
    )
}
