import { useState } from 'react'
import useSWR, { mutate as globalMutate } from 'swr'
import clienteAxios from '../config/axios'
import Producto from '../components/Producto'
import ProductoSkeleton from '../components/ProductoSkeleton'
import { toast } from 'react-toastify'

const vacioForm = { nombre: '', precio: '', imagen: '', categoria_id: '', disponible: true }

export default function Productos() {
  const token = localStorage.getItem('AUTH_TOKEN')
  const headers = { Authorization: `Bearer ${token}` }

  const fetcher = (url) => clienteAxios(url, { headers }).then(r => r.data)

  const { data, isLoading } = useSWR('/api/productos', fetcher, { refreshInterval: 10000 })
  const { data: catData } = useSWR('/api/categorias', fetcher)

  const [modal, setModal] = useState(false)
  const [editando, setEditando] = useState(null) // null = nuevo, objeto = editar
  const [form, setForm] = useState(vacioForm)
  const [guardando, setGuardando] = useState(false)

  const abrirNuevo = () => {
    setEditando(null)
    setForm(vacioForm)
    setModal(true)
  }

  const abrirEditar = (producto) => {
    setEditando(producto)
    setForm({
      nombre: producto.nombre,
      precio: producto.precio,
      imagen: producto.imagen,
      categoria_id: producto.categoria_id,
      disponible: producto.disponible
    })
    setModal(true)
  }

  const cerrarModal = () => {
    setModal(false)
    setEditando(null)
    setForm(vacioForm)
  }

  const handleChange = e => {
    const { name, value, type, checked } = e.target
    setForm(prev => ({ ...prev, [name]: type === 'checkbox' ? checked : value }))
  }

  const handleSubmit = async e => {
    e.preventDefault()
    setGuardando(true)
    try {
      const payload = {
        nombre: form.nombre,
        precio: parseFloat(form.precio),
        imagen: form.imagen,
        categoria_id: parseInt(form.categoria_id),
        disponible: form.disponible
      }

      if (editando) {
        await clienteAxios.put(`/api/productos/${editando.id}`, payload, { headers })
        toast.success('Producto actualizado')
      } else {
        await clienteAxios.post('/api/productos', payload, { headers })
        toast.success('Producto creado')
      }

      globalMutate('/api/productos')
      cerrarModal()
    } catch (error) {
      toast.error('Error al guardar el producto')
    } finally {
      setGuardando(false)
    }
  }

  const handleMarcarDisponible = async (producto) => {
    try {
      await clienteAxios.put(`/api/productos/${producto.id}`, {
        nombre: producto.nombre,
        precio: producto.precio,
        imagen: producto.imagen,
        categoria_id: producto.categoria_id,
        disponible: true
      }, { headers })
      globalMutate('/api/productos')
      toast.success('Producto marcado como disponible')
    } catch {
      toast.error('Error al actualizar')
    }
  }

  const categorias = catData?.data ?? []

  return (
    <div>
      <div className='flex justify-between items-center mb-6'>
        <div>
          <h1 className='text-4xl font-black text-gray-800 dark:text-white'>Productos</h1>
          <p className='text-2xl my-2 text-gray-500 dark:text-gray-300'>Maneja la disponibilidad desde aquí.</p>
        </div>
        <button
          type='button'
          onClick={abrirNuevo}
          className='bg-indigo-600 hover:bg-indigo-800 text-white px-5 py-3 font-bold uppercase rounded'
        >
          + Nuevo Producto
        </button>
      </div>

      {isLoading ? (
        <div className='grid gap-4 grid-cols-1 md:grid-cols-2 xl:grid-cols-3'>
          {[...Array(6)].map((_, i) => <ProductoSkeleton key={i} />)}
        </div>
      ) : (
        <div className='grid gap-4 grid-cols-1 md:grid-cols-2 xl:grid-cols-3'>
          {data?.data?.map(producto => (
            <div key={producto.id} className='border p-3 shadow bg-white'>
              <img
                alt={`imagen ${producto.nombre}`}
                className='w-full h-48 object-cover'
                src={`/img/${producto.imagen}.jpg`}
              />
              <div className='p-3'>
                <h3 className='text-xl font-bold'>{producto.nombre}</h3>
                <p className='mt-2 font-black text-2xl text-amber-500'>
                  ${Number(producto.precio).toFixed(2)}
                </p>
                {!producto.disponible && (
                  <span className='inline-block mt-2 bg-red-100 text-red-700 text-xs font-bold px-2 py-1 rounded'>
                    AGOTADO
                  </span>
                )}
                <div className='flex gap-2 mt-4'>
                  <button
                    type='button'
                    onClick={() => abrirEditar(producto)}
                    className='flex-1 bg-amber-500 hover:bg-amber-600 text-white py-2 font-bold uppercase text-sm rounded'
                  >
                    Editar
                  </button>
                  {producto.disponible ? (
                    <button
                      type='button'
                      onClick={() => globalMutate('/api/productos') || clienteAxios.put(`/api/productos/${producto.id}`, null, { headers }).then(() => { globalMutate('/api/productos'); toast.success('Marcado como agotado') })}
                      className='flex-1 bg-red-500 hover:bg-red-700 text-white py-2 font-bold uppercase text-sm rounded'
                    >
                      Agotado
                    </button>
                  ) : (
                    <button
                      type='button'
                      onClick={() => handleMarcarDisponible(producto)}
                      className='flex-1 bg-green-600 hover:bg-green-800 text-white py-2 font-bold uppercase text-sm rounded'
                    >
                      Disponible
                    </button>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {modal && (
        <div className='fixed inset-0 bg-black/50 flex items-center justify-center z-50'>
          <div className='bg-white rounded-lg p-6 w-full max-w-md mx-4'>
            <h2 className='text-2xl font-bold mb-4'>
              {editando ? 'Editar Producto' : 'Nuevo Producto'}
            </h2>

            <form onSubmit={handleSubmit} className='space-y-4'>
              <div>
                <label className='block text-sm font-bold mb-1'>Nombre</label>
                <input
                  type='text'
                  name='nombre'
                  value={form.nombre}
                  onChange={handleChange}
                  required
                  className='w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-400'
                />
              </div>

              <div>
                <label className='block text-sm font-bold mb-1'>Precio</label>
                <input
                  type='number'
                  step='0.01'
                  min='0.01'
                  name='precio'
                  value={form.precio}
                  onChange={handleChange}
                  required
                  className='w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-400'
                />
              </div>

              <div>
                <label className='block text-sm font-bold mb-1'>Imagen (nombre de archivo sin .jpg)</label>
                <input
                  type='text'
                  name='imagen'
                  value={form.imagen}
                  onChange={handleChange}
                  required
                  placeholder='ej: cafe_capuchino'
                  className='w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-400'
                />
              </div>

              <div>
                <label className='block text-sm font-bold mb-1'>Categoría</label>
                <select
                  name='categoria_id'
                  value={form.categoria_id}
                  onChange={handleChange}
                  required
                  className='w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-400'
                >
                  <option value=''>Seleccionar categoría</option>
                  {categorias.map(cat => (
                    <option key={cat.id} value={cat.id}>{cat.nombre}</option>
                  ))}
                </select>
              </div>

              <div className='flex items-center gap-2'>
                <input
                  type='checkbox'
                  name='disponible'
                  id='disponible'
                  checked={form.disponible}
                  onChange={handleChange}
                  className='w-4 h-4'
                />
                <label htmlFor='disponible' className='text-sm font-bold'>Disponible</label>
              </div>

              <div className='flex gap-3 pt-2'>
                <button
                  type='submit'
                  disabled={guardando}
                  className='flex-1 bg-indigo-600 hover:bg-indigo-800 text-white py-2 font-bold uppercase rounded disabled:opacity-50'
                >
                  {guardando ? 'Guardando...' : (editando ? 'Actualizar' : 'Crear')}
                </button>
                <button
                  type='button'
                  onClick={cerrarModal}
                  className='flex-1 bg-gray-400 hover:bg-gray-600 text-white py-2 font-bold uppercase rounded'
                >
                  Cancelar
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}
