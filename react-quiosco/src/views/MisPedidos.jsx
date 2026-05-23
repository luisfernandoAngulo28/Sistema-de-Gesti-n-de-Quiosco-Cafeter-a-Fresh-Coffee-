import { Link } from 'react-router-dom'
import useSWR from 'swr'
import { CheckCircle2, Clock, UtensilsCrossed, DollarSign, ShoppingBag, Banknote, CreditCard, Smartphone } from 'lucide-react'
import clienteAxios from '../config/axios'
import { formatearDinero } from '../helpers'

const iconosPago = { efectivo: Banknote, tarjeta: CreditCard, transferencia: Smartphone }

export default function MisPedidos() {
  const token = localStorage.getItem('AUTH_TOKEN')
  const fetcher = () => clienteAxios('/api/pedidos/mios', {
    headers: { Authorization: `Bearer ${token}` }
  }).then(r => r.data)

  const { data, isLoading } = useSWR('/api/pedidos/mios', fetcher)

  if (isLoading) return (
    <div>
      <div className='flex items-center gap-3 mb-6'>
        <div className='h-4 w-24 bg-gray-100 rounded animate-pulse' />
      </div>
      <div className='grid grid-cols-3 gap-3 mb-6'>
        {[...Array(3)].map((_, i) => <div key={i} className='bg-white rounded-2xl p-4 border border-gray-100 animate-pulse h-20' />)}
      </div>
      <div className='space-y-3'>
        {[...Array(3)].map((_, i) => <div key={i} className='bg-white rounded-2xl p-5 border border-gray-100 animate-pulse h-32' />)}
      </div>
    </div>
  )

  const pedidos = data?.data ?? []
  const completados = pedidos.filter(p => p.estado).length
  const pendientes = pedidos.filter(p => !p.estado).length
  const totalGastado = pedidos.reduce((acc, p) => acc + p.total, 0)

  return (
    <div>
      <div className='flex items-center gap-3 mb-6'>
        <Link to='/' className='flex items-center gap-1.5 text-sm text-gray-400 dark:text-gray-500 hover:text-indigo-600 dark:hover:text-indigo-400 transition-colors font-medium'>
          <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
            <path strokeLinecap="round" strokeLinejoin="round" d="M15 19l-7-7 7-7" />
          </svg>
          Volver al menú
        </Link>
      </div>

      <div className='mb-6'>
        <h1 className='text-2xl font-bold text-gray-800 dark:text-gray-100'>Mis Pedidos</h1>
        <p className='text-gray-400 dark:text-gray-500 text-sm mt-1'>
          {pedidos.length === 0 ? 'Sin pedidos aún' : `${pedidos.length} ${pedidos.length === 1 ? 'pedido realizado' : 'pedidos realizados'}`}
        </p>
      </div>

      {/* Stats cards */}
      {pedidos.length > 0 && (
        <div className='grid grid-cols-3 gap-3 mb-6'>
          <div className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 p-4 flex items-center gap-3'>
            <div className='w-9 h-9 bg-amber-50 dark:bg-amber-900/20 rounded-xl flex items-center justify-center flex-shrink-0'>
              <DollarSign className='w-5 h-5 text-amber-500' />
            </div>
            <div>
              <p className='text-xs text-gray-400 dark:text-gray-500 font-medium'>Total gastado</p>
              <p className='text-lg font-black text-amber-500'>{formatearDinero(totalGastado)}</p>
            </div>
          </div>
          <div className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 p-4 flex items-center gap-3'>
            <div className='w-9 h-9 bg-green-50 dark:bg-green-900/20 rounded-xl flex items-center justify-center flex-shrink-0'>
              <CheckCircle2 className='w-5 h-5 text-green-500' />
            </div>
            <div>
              <p className='text-xs text-gray-400 dark:text-gray-500 font-medium'>Completados</p>
              <p className='text-lg font-black text-green-500'>{completados}</p>
            </div>
          </div>
          <div className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 p-4 flex items-center gap-3'>
            <div className='w-9 h-9 bg-indigo-50 dark:bg-indigo-900/20 rounded-xl flex items-center justify-center flex-shrink-0'>
              <Clock className='w-5 h-5 text-indigo-500' />
            </div>
            <div>
              <p className='text-xs text-gray-400 dark:text-gray-500 font-medium'>Pendientes</p>
              <p className='text-lg font-black text-indigo-500'>{pendientes}</p>
            </div>
          </div>
        </div>
      )}

      {pedidos.length === 0 ? (
        <div className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 py-20 text-center'>
          <div className='w-16 h-16 rounded-2xl bg-gray-50 dark:bg-gray-800 flex items-center justify-center mx-auto mb-4'>
            <ShoppingBag className='w-8 h-8 text-gray-200 dark:text-gray-600' />
          </div>
          <p className='text-gray-400 dark:text-gray-500 font-semibold'>No tienes pedidos aún</p>
          <Link to='/' className='inline-block mt-3 text-sm text-indigo-600 font-semibold hover:text-indigo-800 dark:hover:text-indigo-400'>
            Ir al menú →
          </Link>
        </div>
      ) : (
        <div className='space-y-3'>
          {pedidos.map(pedido => (
            <div key={pedido.id} className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 overflow-hidden hover:shadow-sm transition-shadow'>
              <div className='px-5 py-3 flex justify-between items-center border-b border-gray-50 dark:border-gray-700/50'>
                <div>
                  <div className='flex items-center gap-2'>
                    <p className='text-xs text-gray-400 dark:text-gray-500'>Pedido #{pedido.id}</p>
                    {pedido.mesa && (
                      <span className='text-xs bg-indigo-50 dark:bg-indigo-900/30 text-indigo-600 dark:text-indigo-400 font-semibold px-2 py-0.5 rounded-full flex items-center gap-1'>
                        <UtensilsCrossed className='w-3 h-3' />
                        Mesa {pedido.mesa.numero}
                      </span>
                    )}
                  </div>
                  <p className='text-xs text-gray-400 dark:text-gray-500 mt-0.5'>
                    {new Date(pedido.created_at).toLocaleDateString('es-MX', {
                      year: 'numeric', month: 'short', day: 'numeric',
                      hour: '2-digit', minute: '2-digit'
                    })}
                  </p>
                </div>
                <span className={`text-xs font-bold px-3 py-1 rounded-full flex items-center gap-1 ${
                  pedido.estado
                    ? 'bg-green-50 dark:bg-green-900/20 text-green-600 dark:text-green-400'
                    : 'bg-amber-50 dark:bg-amber-900/20 text-amber-600 dark:text-amber-400'
                }`}>
                  {pedido.estado
                    ? <><CheckCircle2 className='w-3.5 h-3.5' /> Completado</>
                    : <><Clock className='w-3.5 h-3.5' /> Pendiente</>
                  }
                </span>
              </div>

              <div className='px-5 py-3 flex items-center justify-between'>
                <div className='flex flex-wrap gap-1.5'>
                  {pedido.productos.map(p => (
                    <div key={p.id} className='flex items-center gap-1.5 bg-gray-50 dark:bg-gray-800 rounded-lg px-2.5 py-1'>
                      <span className='text-xs text-gray-600 dark:text-gray-300 font-medium'>{p.nombre}</span>
                      <span className='text-xs text-gray-400 dark:text-gray-500 font-bold'>×{p.pivot.cantidad}</span>
                    </div>
                  ))}
                </div>
                <div className='ml-4 flex-shrink-0 text-right'>
                  <p className='font-black text-lg text-gray-800 dark:text-gray-100'>{formatearDinero(pedido.total)}</p>
                  {pedido.pago && (() => {
                    const IconoPago = iconosPago[pedido.pago.metodo] ?? Banknote
                    return (
                      <span className='inline-flex items-center gap-1 text-xs text-gray-400 dark:text-gray-500 mt-0.5 capitalize'>
                        <IconoPago className='w-3 h-3' />
                        {pedido.pago.metodo}
                      </span>
                    )
                  })()}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}
