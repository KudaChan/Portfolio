const layout = ({
    children
}: {
    children: React.ReactNode
}) => {
  return (
      <div className="h-full flex items-center justify-center bg-blue-300">{ children }</div>
  )
}

export default layout