import { useLocation, useNavigate } from 'react-router-dom'
import { CheckCircle, UtensilsCrossed, Banknote, CreditCard, Smartphone, LogOut, RefreshCw } from 'lucide-react'
import { useAuth } from '../hooks/useAuth'
import { formatearDinero } from '../helpers'

export default function Gracias() {
    const { state } = useLocation()
    const navigate  = useNavigate()
    const { logout } = useAuth({})

    const pedido      = state?.pedido      ?? []
    const total       = state?.total       ?? 0
    const numerMesa   = state?.numerMesa   ?? null
    const metodoPago  = state?.metodoPago  ?? ''

    const iconoMetodo = {
        efectivo:       Banknote,
        tarjeta:        CreditCard,
        transferencia:  Smartphone,
    }
    const IconoPago = iconoMetodo[metodoPago] ?? Banknote

    return (
        <div className="min-h-screen bg-gradient-to-br from-indigo-50 to-white dark:from-gray-950 dark:to-gray-900 flex items-center justify-center p-6">
            <div className="bg-white dark:bg-gray-900 rounded-2xl shadow-xl max-w-md w-full overflow-hidden">

                {/* Header */}
                <div className="bg-indigo-600 px-8 py-10 text-center">
                    <div className="w-20 h-20 bg-white/20 rounded-full flex items-center justify-center mx-auto mb-4">
                        <CheckCircle className="w-10 h-10 text-white" />
                    </div>
                    <h1 className="text-2xl font-black text-white">¡Pedido Confirmado!</h1>
                    <p className="text-indigo-200 mt-1 text-sm">Tu orden está siendo preparada</p>
                    <div className="mt-4 inline-flex items-center gap-2 bg-green-500 text-white text-xs font-black px-4 py-1.5 rounded-full shadow-md">
                        <CheckCircle className="w-3.5 h-3.5" />
                        PAGADO
                    </div>
                </div>

                {/* Cuerpo */}
                <div className="px-8 py-6 space-y-5">

                    {/* Info mesa y pago */}
                    <div className="flex gap-3">
                        {numerMesa && (
                            <div className="flex-1 bg-indigo-50 dark:bg-indigo-900/30 rounded-xl p-3 text-center">
                                <div className="flex items-center justify-center gap-1 mb-1">
                                    <UtensilsCrossed className="w-3.5 h-3.5 text-indigo-400" />
                                    <p className="text-xs text-indigo-400 font-semibold uppercase tracking-wide">Mesa</p>
                                </div>
                                <p className="text-2xl font-black text-indigo-700 dark:text-indigo-400">{numerMesa}</p>
                            </div>
                        )}
                        {metodoPago && (
                            <div className="flex-1 bg-green-50 dark:bg-green-900/20 border border-green-200 dark:border-green-800/50 rounded-xl p-3 text-center">
                                <div className="flex items-center justify-center gap-1 mb-1">
                                    <IconoPago className="w-3.5 h-3.5 text-green-500" />
                                    <p className="text-xs text-green-600 dark:text-green-400 font-semibold uppercase tracking-wide">Pago</p>
                                </div>
                                <p className="text-base font-bold text-green-700 dark:text-green-400 capitalize">{metodoPago}</p>
                                <p className="text-xs text-green-500 font-semibold mt-0.5">✓ Registrado</p>
                            </div>
                        )}
                    </div>

                    {/* Lista de productos */}
                    {pedido.length > 0 && (
                        <div>
                            <p className="text-xs font-bold text-gray-400 dark:text-gray-500 uppercase tracking-wider mb-2">Tu pedido</p>
                            <div className="space-y-2">
                                {pedido.map(p => (
                                    <div key={p.id} className="flex items-center justify-between">
                                        <div className="flex items-center gap-2">
                                            <span className="text-xs bg-indigo-100 dark:bg-indigo-900/50 text-indigo-700 dark:text-indigo-300 font-bold rounded-full w-5 h-5 flex items-center justify-center">
                                                {p.cantidad}
                                            </span>
                                            <span className="text-sm text-gray-700 dark:text-gray-200 truncate max-w-[180px]">{p.nombre}</span>
                                        </div>
                                        <span className="text-sm font-semibold text-gray-600 dark:text-gray-400">
                                            {formatearDinero(p.precio * p.cantidad)}
                                        </span>
                                    </div>
                                ))}
                            </div>
                        </div>
                    )}

                    {/* Total */}
                    <div className="border-t border-gray-100 dark:border-gray-700/50 pt-4 flex items-center justify-between">
                        <span className="text-sm font-bold text-gray-500 dark:text-gray-400">Total pagado</span>
                        <span className="text-2xl font-black text-indigo-700 dark:text-indigo-400">{formatearDinero(total)}</span>
                    </div>

                    {/* Botones */}
                    <div className="flex flex-col gap-2 pt-1">
                        <button
                            onClick={() => navigate('/')}
                            className="w-full py-3 bg-indigo-600 hover:bg-indigo-700 text-white font-bold rounded-xl transition-colors flex items-center justify-center gap-2"
                        >
                            <RefreshCw className="w-4 h-4" />
                            Hacer otro pedido
                        </button>
                        <button
                            onClick={logout}
                            className="w-full py-3 bg-gray-100 dark:bg-gray-800 hover:bg-gray-200 dark:hover:bg-gray-700 text-gray-500 dark:text-gray-400 font-semibold rounded-xl transition-colors text-sm flex items-center justify-center gap-2"
                        >
                            <LogOut className="w-4 h-4" />
                            Cerrar sesión
                        </button>
                    </div>
                </div>
            </div>
        </div>
    )
}
