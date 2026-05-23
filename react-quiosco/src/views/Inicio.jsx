import { useState, useMemo } from 'react'
import useSWR from 'swr'
import Producto from '../components/Producto'
import ProductoSkeleton from '../components/ProductoSkeleton'
import clienteAxios from '../config/axios'
import useQuisco from '../hooks/useQuiosco'

export default function Inicio() {
  const { categoriaActual } = useQuisco()
  const [busqueda, setBusqueda] = useState('')
  const [filtroPrecio, setFiltroPrecio] = useState('todos')

  const filtrosPrecio = [
    { key: 'todos', label: 'Todos' },
    { key: 'bajo', label: 'Menos de $30' },
    { key: 'medio', label: '$30 – $60' },
    { key: 'alto', label: 'Más de $60' },
  ]

  const token = localStorage.getItem('AUTH_TOKEN')
  const fetcher = () => clienteAxios('/api/productos', {
    headers: { Authorization: `Bearer ${token}` }
  }).then(data => data.data)

  const { data, isLoading } = useSWR('/api/productos', fetcher, { refreshInterval: 5000 })

  if (isLoading || !categoriaActual?.id) return (
    <div>
      <div className='mb-6 flex flex-col sm:flex-row sm:items-center justify-between gap-4'>
        <div>
          <div className='h-8 w-32 bg-gray-200 rounded-lg animate-pulse mb-2' />
          <div className='h-4 w-48 bg-gray-100 rounded animate-pulse' />
        </div>
        <div className='h-10 w-full sm:w-64 bg-gray-100 rounded-xl animate-pulse' />
      </div>
      <div className='grid gap-4 grid-cols-1 md:grid-cols-2 xl:grid-cols-3'>
        {[...Array(6)].map((_, i) => <ProductoSkeleton key={i} />)}
      </div>
    </div>
  )

  const productosFiltrados = data.data
    .filter(p => p.categoria_id === categoriaActual.id)
    .filter(p => p.nombre.toLowerCase().includes(busqueda.toLowerCase()))
    .filter(p => {
      if (filtroPrecio === 'bajo') return p.precio < 30
      if (filtroPrecio === 'medio') return p.precio >= 30 && p.precio <= 60
      if (filtroPrecio === 'alto') return p.precio > 60
      return true
    })

  return (
    <div>
      <div className='mb-6 flex flex-col sm:flex-row sm:items-center justify-between gap-4'>
        <div>
          <h1 className='text-2xl font-bold text-gray-800 dark:text-gray-100'>{categoriaActual.nombre}</h1>
          <p className='text-gray-400 dark:text-gray-500 text-sm mt-0.5'>
            {productosFiltrados.length} {productosFiltrados.length === 1 ? 'producto' : 'productos'}
            {busqueda && ` para "${busqueda}"`}
          </p>
        </div>

        <div className='flex-shrink-0 flex flex-col gap-2 sm:items-end'>
        <div className='flex gap-1 flex-wrap sm:justify-end'>
          {filtrosPrecio.map(f => (
            <button
              key={f.key}
              type='button'
              onClick={() => setFiltroPrecio(f.key)}
              className={`text-xs font-semibold px-3 py-1.5 rounded-full border transition-all duration-150
                ${filtroPrecio === f.key
                  ? 'bg-amber-500 text-white border-amber-500'
                  : 'bg-white dark:bg-gray-800 text-gray-500 dark:text-gray-400 border-gray-200 dark:border-gray-600 hover:border-amber-300 hover:text-amber-600'
                }`}
            >
              {f.label}
            </button>
          ))}
        </div>
        <div className='relative w-full sm:w-60'>
          <svg xmlns="http://www.w3.org/2000/svg" className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
          <input
            type="text"
            placeholder={`Buscar en ${categoriaActual.nombre}...`}
            value={busqueda}
            onChange={e => setBusqueda(e.target.value)}
            className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-gray-200 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-800 dark:text-gray-100 placeholder-gray-400 dark:placeholder-gray-500 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:border-transparent transition"
          />
          {busqueda && (
            <button
              onClick={() => setBusqueda('')}
              className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600"
            >
              <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          )}
        </div>
        </div>
      </div>

      {productosFiltrados.length === 0 ? (
        <div className='bg-white dark:bg-gray-900 rounded-2xl border border-gray-100 dark:border-gray-700/50 py-16 text-center'>
          <p className='text-gray-400 dark:text-gray-500 text-sm'>No se encontraron productos para <strong>"{busqueda}"</strong></p>
          <button onClick={() => setBusqueda('')} className='mt-3 text-indigo-600 text-sm font-semibold hover:text-indigo-800 dark:hover:text-indigo-400'>
            Limpiar búsqueda
          </button>
        </div>
      ) : (
        <div className='grid gap-4 grid-cols-1 md:grid-cols-2 xl:grid-cols-3'>
          {productosFiltrados.map(producto => (
            <Producto key={producto.imagen} producto={producto} botonAgregar={true} />
          ))}
        </div>
      )}
    </div>
  )
}
