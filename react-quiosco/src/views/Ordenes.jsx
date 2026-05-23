import useSWR, { mutate } from 'swr'
import { CheckCircle2, UtensilsCrossed, Banknote, CreditCard, Smartphone, Clock } from 'lucide-react'
import useQuiosco from '../hooks/useQuiosco'
import clienteAxios from '../config/axios'
import { formatearDinero } from '../helpers'

const iconosPago = { efectivo: Banknote, tarjeta: CreditCard, transferencia: Smartphone }

export default function Ordenes() {
    const token = localStorage.getItem('AUTH_TOKEN')
    const fetcher = () => clienteAxios('/api/pedidos', {
        headers: { Authorization: `Bearer ${token}` }
    })

    const { data, isLoading } = useSWR('/api/pedidos', fetcher, { refreshInterval: 3000 })
    const { handleClickCompletarPedido } = useQuiosco()

    const completar = async (id) => {
        await handleClickCompletarPedido(id)
        mutate('/api/pedidos')
        mutate('/api/mesas')
    }

    if (isLoading) return (
        <div className='grid grid-cols-1 md:grid-cols-2 gap-4 mt-4'>
            {[...Array(4)].map((_, i) => (
                <div key={i} className='bg-white dark:bg-gray-900 rounded-2xl p-5 border border-gray-100 dark:border-gray-700/50 animate-pulse space-y-3'>
                    <div className='h-4 bg-gray-100 dark:bg-gray-800 rounded w-1/3' />
                    <div className='h-3 bg-gray-100 dark:bg-gray-800 rounded w-full' />
                    <div className='h-3 bg-gray-100 dark:bg-gray-800 rounded w-2/3' />
                    <div className='h-9 bg-gray-100 dark:bg-gray-800 rounded-xl mt-4' />
                </div>
            ))}
        </div>
    )

    const pedidos = data?.data?.data ?? []

    return (
        <div>
            <div className='mb-8 flex items-center justify-between'>
                <div>
                    <h1 className='text-3xl font-bold text-gray-800 dark:text-gray-100'>Órdenes Activas</h1>
                    <p className='text-gray-400 dark:text-gray-500 text-sm mt-1 flex items-center gap-1.5'>
                        <Clock className='w-3.5 h-3.5' />
                        {pedidos.length === 0
                            ? 'No hay órdenes pendientes'
                            : `${pedidos.length} ${pedidos.length === 1 ? 'orden pendiente' : 'órdenes pendientes'}`}
                    </p>
                </div>
            </div>

            {pedidos.length === 0 ? (
                <div className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 p-16 text-center'>
                    <div className='w-16 h-16 rounded-full bg-green-50 dark:bg-green-900/20 flex items-center justify-center mx-auto mb-4'>
                        <CheckCircle2 className='w-8 h-8 text-green-400' />
                    </div>
                    <p className='text-gray-500 dark:text-gray-400 font-medium'>¡Todo al día! No hay órdenes pendientes.</p>
                </div>
            ) : (
                <div className='grid grid-cols-1 md:grid-cols-2 gap-4'>
                    {pedidos.map(pedido => {
                        const IconoPago = pedido.pago ? (iconosPago[pedido.pago.metodo] ?? Banknote) : null
                        return (
                            <div key={pedido.id} className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 overflow-hidden hover:shadow-md transition-shadow flex flex-col'>

                                {/* Header */}
                                <div className='px-5 py-4 border-b border-gray-50 dark:border-gray-700/50 flex justify-between items-center'>
                                    <div>
                                        <p className='text-xs text-gray-400 dark:text-gray-500'>Pedido #{pedido.id}</p>
                                        <p className='font-bold text-gray-800 dark:text-gray-100'>{pedido.user.name}</p>
                                    </div>
                                    <div className='flex items-center gap-2'>
                                        {pedido.mesa && (
                                            <span className='text-xs bg-indigo-50 dark:bg-indigo-900/30 text-indigo-600 dark:text-indigo-400 font-semibold px-2.5 py-1 rounded-full flex items-center gap-1'>
                                                <UtensilsCrossed className='w-3 h-3' />
                                                Mesa {pedido.mesa.numero}
                                            </span>
                                        )}
                                        <span className='text-xs bg-amber-50 dark:bg-amber-900/20 text-amber-600 dark:text-amber-400 font-bold px-3 py-1 rounded-full border border-amber-200 dark:border-amber-800/50'>
                                            Pendiente
                                        </span>
                                    </div>
                                </div>

                                {/* Productos */}
                                <div className='px-5 py-3 space-y-2 flex-1'>
                                    {pedido.productos.map(producto => (
                                        <div key={producto.id} className='flex justify-between items-center text-sm'>
                                            <span className='text-gray-700 dark:text-gray-300 truncate pr-2'>{producto.nombre}</span>
                                            <span className='text-gray-400 dark:text-gray-500 font-bold flex-shrink-0 bg-gray-50 dark:bg-gray-800 px-2 py-0.5 rounded-full text-xs'>
                                                ×{producto.pivot.cantidad}
                                            </span>
                                        </div>
                                    ))}
                                </div>

                                {/* Footer */}
                                <div className='px-5 py-4 border-t border-gray-50 dark:border-gray-700/50 flex items-center justify-between'>
                                    <div>
                                        <p className='text-xs text-gray-400 dark:text-gray-500'>Total</p>
                                        <p className='font-black text-xl text-gray-800 dark:text-gray-100'>{formatearDinero(pedido.total)}</p>
                                        {pedido.pago && IconoPago && (
                                            <span className='inline-flex items-center gap-1 text-xs text-green-600 dark:text-green-400 font-semibold mt-0.5'>
                                                <IconoPago className='w-3 h-3' />
                                                {pedido.pago.metodo} · Pagado
                                            </span>
                                        )}
                                        {!pedido.pago && (
                                            <span className='text-xs text-amber-500 font-semibold mt-0.5 block'>
                                                Pago pendiente
                                            </span>
                                        )}
                                    </div>
                                    <button
                                        type='button'
                                        onClick={() => completar(pedido.id)}
                                        className='bg-green-500 hover:bg-green-600 active:scale-95 text-white px-5 py-2.5 rounded-xl font-bold text-sm transition-all duration-150 flex items-center gap-2 shadow-sm shadow-green-200 dark:shadow-none'
                                    >
                                        <CheckCircle2 className='w-4 h-4' />
                                        Completar
                                    </button>
                                </div>
                            </div>
                        )
                    })}
                </div>
            )}
        </div>
    )
}
