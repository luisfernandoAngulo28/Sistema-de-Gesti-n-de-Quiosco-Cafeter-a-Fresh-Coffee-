import useSWR, { mutate } from 'swr'
import { UtensilsCrossed, Users, CheckCircle2, XCircle, RefreshCw } from 'lucide-react'
import clienteAxios from '../config/axios'

export default function AdminMesas() {
    const token = localStorage.getItem('AUTH_TOKEN')
    const get = url => clienteAxios(url, { headers: { Authorization: `Bearer ${token}` } })

    const { data: mesasRaw, isLoading } = useSWR('admin-mesas',   () => get('/api/mesas'),   { refreshInterval: 5000 })
    const { data: pedidosRaw }           = useSWR('admin-pedidos', () => get('/api/pedidos'), { refreshInterval: 5000 })

    // Normaliza la respuesta axios → array
    const mesas   = mesasRaw?.data?.data   ?? mesasRaw?.data   ?? []
    const pedidos = pedidosRaw?.data?.data ?? pedidosRaw?.data ?? []

    const toggleDisponible = async (mesa) => {
        await clienteAxios.put(`/api/mesas/${mesa.id}`, {
            numero: mesa.numero,
            capacidad: mesa.capacidad,
            disponible: !mesa.disponible
        }, { headers: { Authorization: `Bearer ${token}` } })
        mutate('admin-mesas')
    }

    const liberarMesa = async (mesa) => {
        const pedidosPendientes = pedidos.filter(
            p => p.mesa?.id === mesa.id && !p.estado
        )
        await Promise.all(
            pedidosPendientes.map(p =>
                clienteAxios.put(`/api/pedidos/${p.id}`, null, {
                    headers: { Authorization: `Bearer ${token}` }
                })
            )
        )
        mutate('admin-mesas')
        mutate('admin-pedidos')
    }

    if (isLoading) return (
        <div>
            <div className='mb-8'>
                <div className='h-8 w-48 bg-gray-200 dark:bg-gray-700 rounded-lg animate-pulse mb-2' />
                <div className='h-4 w-32 bg-gray-100 dark:bg-gray-800 rounded animate-pulse' />
            </div>
            <div className='grid grid-cols-2 md:grid-cols-4 gap-4'>
                {[...Array(8)].map((_, i) => (
                    <div key={i} className='bg-white dark:bg-gray-900 rounded-2xl p-5 border border-gray-100 dark:border-gray-700/50 animate-pulse h-44' />
                ))}
            </div>
        </div>
    )

    const mesasList = mesas ?? []
    const ocupadas = mesasList.filter(m => m.ocupada).length
    const libres   = mesasList.filter(m => !m.ocupada && m.disponible).length

    return (
        <div>
            <div className='mb-6 flex flex-col sm:flex-row sm:items-center justify-between gap-3'>
                <div>
                    <h1 className='text-3xl font-bold text-gray-800 dark:text-gray-100'>Mesas</h1>
                    <p className='text-gray-400 dark:text-gray-500 text-sm mt-1'>
                        {ocupadas} ocupada{ocupadas !== 1 ? 's' : ''} · {libres} libre{libres !== 1 ? 's' : ''}
                    </p>
                </div>
                <button
                    onClick={() => { mutate('admin-mesas'); mutate('admin-pedidos') }}
                    className='flex items-center gap-2 text-sm font-semibold text-indigo-600 hover:text-indigo-800 dark:text-indigo-400 dark:hover:text-indigo-300 transition-colors'
                >
                    <RefreshCw className='w-4 h-4' />
                    Actualizar
                </button>
            </div>

            {/* Leyenda */}
            <div className='flex items-center gap-4 mb-5 text-xs font-semibold'>
                <span className='flex items-center gap-1.5 text-green-600 dark:text-green-400'>
                    <span className='w-3 h-3 rounded-full bg-green-400 inline-block' /> Libre
                </span>
                <span className='flex items-center gap-1.5 text-red-500 dark:text-red-400'>
                    <span className='w-3 h-3 rounded-full bg-red-400 inline-block' /> Ocupada
                </span>
                <span className='flex items-center gap-1.5 text-gray-400 dark:text-gray-500'>
                    <span className='w-3 h-3 rounded-full bg-gray-300 dark:bg-gray-600 inline-block' /> No disponible
                </span>
            </div>

            <div className='grid grid-cols-2 md:grid-cols-4 gap-4'>
                {mesasList.map(mesa => {
                    const pedidosMesa = (pedidos ?? []).filter(p => p.mesa?.id === mesa.id && !p.estado)
                    const isOcupada = mesa.ocupada
                    const isDisponible = mesa.disponible

                    return (
                        <div
                            key={mesa.id}
                            className={`rounded-2xl border overflow-hidden transition-all duration-200 flex flex-col
                                ${isOcupada
                                    ? 'bg-red-50 dark:bg-red-900/10 border-red-200 dark:border-red-800/50'
                                    : isDisponible
                                        ? 'bg-white dark:bg-gray-900 border-gray-100 dark:border-gray-700/50'
                                        : 'bg-gray-50 dark:bg-gray-800/50 border-gray-200 dark:border-gray-700'
                                }`}
                        >
                            {/* Header de la tarjeta */}
                            <div className={`px-4 pt-4 pb-3 flex items-center justify-between`}>
                                <div className='flex items-center gap-2'>
                                    <div className={`w-10 h-10 rounded-xl flex items-center justify-center font-black text-lg
                                        ${isOcupada
                                            ? 'bg-red-100 dark:bg-red-900/30 text-red-600 dark:text-red-400'
                                            : isDisponible
                                                ? 'bg-green-100 dark:bg-green-900/30 text-green-600 dark:text-green-400'
                                                : 'bg-gray-200 dark:bg-gray-700 text-gray-400 dark:text-gray-500'
                                        }`}
                                    >
                                        {mesa.numero}
                                    </div>
                                    <div>
                                        <p className='text-xs font-bold text-gray-500 dark:text-gray-400'>Mesa</p>
                                        <div className='flex items-center gap-1 text-xs text-gray-400 dark:text-gray-500'>
                                            <Users className='w-3 h-3' />
                                            {mesa.capacidad} pers.
                                        </div>
                                    </div>
                                </div>
                                <span className={`text-xs font-bold px-2 py-0.5 rounded-full
                                    ${isOcupada
                                        ? 'bg-red-100 dark:bg-red-900/30 text-red-600 dark:text-red-400'
                                        : isDisponible
                                            ? 'bg-green-100 dark:bg-green-900/30 text-green-600 dark:text-green-400'
                                            : 'bg-gray-200 dark:bg-gray-700 text-gray-500 dark:text-gray-400'
                                    }`}
                                >
                                    {isOcupada ? 'Ocupada' : isDisponible ? 'Libre' : 'No disp.'}
                                </span>
                            </div>

                            {/* Pedidos activos */}
                            {pedidosMesa.length > 0 && (
                                <div className='px-4 pb-2 space-y-1'>
                                    {pedidosMesa.map(p => (
                                        <div key={p.id} className='text-xs bg-white dark:bg-gray-800 rounded-lg px-2.5 py-1.5 border border-red-100 dark:border-red-900/30'>
                                            <span className='font-semibold text-gray-600 dark:text-gray-300'>Pedido #{p.id}</span>
                                            <span className='text-gray-400 dark:text-gray-500 ml-1'>· {p.user?.name}</span>
                                        </div>
                                    ))}
                                </div>
                            )}

                            {/* Botones */}
                            <div className='mt-auto px-4 pb-4 pt-2 flex flex-col gap-2'>
                                {isOcupada && (
                                    <button
                                        type='button'
                                        onClick={() => liberarMesa(mesa)}
                                        className='w-full py-2 bg-green-500 hover:bg-green-600 active:scale-95 text-white text-xs font-bold rounded-xl transition-all duration-150 flex items-center justify-center gap-1.5'
                                    >
                                        <CheckCircle2 className='w-3.5 h-3.5' />
                                        Liberar mesa
                                    </button>
                                )}
                                <button
                                    type='button'
                                    onClick={() => toggleDisponible(mesa)}
                                    className={`w-full py-2 text-xs font-bold rounded-xl transition-all duration-150 active:scale-95 flex items-center justify-center gap-1.5
                                        ${isDisponible
                                            ? 'bg-gray-100 dark:bg-gray-800 hover:bg-gray-200 dark:hover:bg-gray-700 text-gray-500 dark:text-gray-400'
                                            : 'bg-indigo-50 dark:bg-indigo-900/30 hover:bg-indigo-100 dark:hover:bg-indigo-900/50 text-indigo-600 dark:text-indigo-400'
                                        }`}
                                >
                                    {isDisponible
                                        ? <><XCircle className='w-3.5 h-3.5' /> Deshabilitar</>
                                        : <><CheckCircle2 className='w-3.5 h-3.5' /> Habilitar</>
                                    }
                                </button>
                            </div>
                        </div>
                    )
                })}
            </div>
        </div>
    )
}
