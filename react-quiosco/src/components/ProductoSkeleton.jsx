export default function ProductoSkeleton() {
  return (
    <div className="bg-white rounded-2xl border border-gray-100 overflow-hidden animate-pulse">
      <div className="h-52 bg-gradient-to-br from-gray-100 to-gray-200 rounded-t-2xl" />
      <div className="p-4">
        <div className="h-4 bg-gray-100 rounded-lg w-4/5 mb-2" />
        <div className="h-3 bg-gray-100 rounded-lg w-3/5 mb-4" />
        <div className="flex items-center justify-between pt-3 border-t border-gray-50">
          <div className="h-6 bg-gray-100 rounded-lg w-20" />
          <div className="h-8 bg-gray-100 rounded-xl w-20" />
        </div>
      </div>
    </div>
  )
}
